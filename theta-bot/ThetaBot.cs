using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.InlineKeyboardButtons;

namespace theta_bot
{
    public class ThetaBot
    {
        private readonly Dictionary<long, int> userLevelsCache = 
            new Dictionary<long,int>();
        private readonly Dictionary<int, string> taskAnswersCache =
            new Dictionary<int, string>();
        
        private readonly Dictionary<string, Action<long>> commands;
        
        private readonly Random random = new Random();
        private readonly TelegramBotClient bot;
        private readonly IDataProvider database;
        private readonly ILevel[] levels;

        public ThetaBot(TelegramBotClient bot, IDataProvider database, params ILevel[] levels)
        {
            this.bot = bot;
            this.database = database;
            this.levels = levels;
            commands = new Dictionary<string, Action<long>>
            {
                {"Дай задачу", async userId => await SendNewTask(userId)},
                {"Хочу сложнее", async userId =>
                {
                    if (CanIncreaseLevel(userId))
                    {
                        IncreaseLevel(userId);
                        await SendMessageAsync(userId, "Уровень повышен");
                    }
                    else await SendMessageAsync(userId, "Пока что повысить уровень нельзя");
                }}
            };

            bot.OnMessage += MessageHandler;
            bot.OnCallbackQuery += AnswerHandler;
            
            bot.StartReceiving();
            Console.ReadLine();
            bot.StopReceiving();
        }
        
        #region Handlers
        private async void AnswerHandler(object sender, CallbackQueryEventArgs e)
        {
            var message = e.CallbackQuery.Message;
            var userId = message.Chat.Id;
            var button = JsonConvert
                .DeserializeObject<ButtonInfo>(e.CallbackQuery.Data);
            bool correct = button.Answer == GetAnswer(button.TaskId);

            database.SetSolved(button.TaskId, correct);
            if (CanIncreaseLevel(userId))
                await SendMessageAsync(userId,
                    "Отлично справляешься! Если хочешь, можешь взять задачи посложнее.");
            await CheckMessageSolvedAsync(message, correct, button.Answer);
        }
        
        private async void MessageHandler(object sender, MessageEventArgs e)
        {
            var message = e.Message.Text;
            var userId = e.Message.Chat.Id;
            if (commands.ContainsKey(message))
                commands[message](userId);
            else
                await SendMessageAsync(userId,
                    "Привет. Нажми на кнопку, чтобы получить задачу");
        }
#endregion

        private void IncreaseLevel(long userId)
        {
            var level = GetLevel(userId);
            database.SetLevel(userId, level + 1);
            userLevelsCache[userId]++;
            var id = database.AddTask(userId, "");
            database.SetSolved(id, false);
        }
        
        private InlineKeyboardMarkup GetReplyMarkup(Task task, int taskId) =>
            new InlineKeyboardMarkup(
                task
                    .GetOptions(random, 4)
                    .Select(option => 
                        new InlineKeyboardCallbackButton(
                            option,
                            JsonConvert.SerializeObject(
                                new ButtonInfo(option, taskId))))
                    .ToArray<InlineKeyboardButton>());
        
        private bool CanIncreaseLevel(long userId)
        {
            var level = GetLevel(userId);
            return level + 1 < levels.Length &&
                   levels[level].IsFinished(database, userId);
        }
        
        #region SendOrEdit
        private async Task<Message> CheckMessageSolvedAsync(Message message, bool correct, string answer)
        {
            var builder = new StringBuilder(message.Text);
            builder.Insert(0, "```\n");
            builder.Append("```\n\n");
            builder.Append(answer);
            builder.Append(" - ");
            builder.Append(correct ? "Верно" : "Ответ неверный");
            return await bot.EditMessageTextAsync(
                message.Chat.Id,
                message.MessageId,
                builder.ToString(),
                ParseMode.Markdown);
        }
        
        private async Task<Message> SendNewTask(long userId)
        {
            var exercise = levels[GetLevel(userId)].Generate(random);
            var taskId = database.AddTask(userId, exercise.Complexity.Value);
            return await bot.SendTextMessageAsync(
                userId,
                exercise.Message,
                ParseMode.Markdown,
                replyMarkup: GetReplyMarkup(exercise, taskId));
        }
        
        private async Task<Message> SendMessageAsync(long userId, string message)
        {
            var buttons = new List<string> {"Дай задачу"};
            if (CanIncreaseLevel(userId))
                buttons.Add("Хочу сложнее");
            return await bot.SendTextMessageAsync(
                userId,
                message,
                replyMarkup: new ReplyKeyboardMarkup(
                    buttons
                        .Select(str => new[] {new KeyboardButton(str)})
                        .ToArray(), 
                    true));
        }
        #endregion

        #region Cache
        private int GetLevel(long userId)
        {
            if (userLevelsCache.ContainsKey(userId))
                return userLevelsCache[userId];
            var level = database.GetLevel(userId);
            if (level != -1) 
                return userLevelsCache[userId] = level;
            database.SetLevel(userId, 0);
            return userLevelsCache[userId] = 0;
        }

        private string GetAnswer(int taskId)
        {
            if (taskAnswersCache.ContainsKey(taskId))
                return taskAnswersCache[taskId];
            return taskAnswersCache[taskId] = database.GetAnswer(taskId);
        }
        #endregion
    }
}