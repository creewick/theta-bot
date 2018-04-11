using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
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
        private readonly Dictionary<string, Action<long>> commands;

        private readonly Dictionary<string, string> answersCache = new Dictionary<string, string>();
        private readonly Dictionary<long, int> levelCache = new Dictionary<long, int>();
        
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
                {   "/start", SendNewTask},
                {   "Give a task", SendNewTask},
                {
                    "Level up", userId =>
                    {
                        if (CanIncreaseLevel(userId))
                        {
                            IncreaseLevel(userId);
                            SendMessage(userId, "Level-up");
                        }
                        else SendMessage(userId, "You can't level-up yet");
                    }
                }
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
            await Task.Factory.StartNew(async () =>
            {
                var timer = Stopwatch.StartNew();
                var message = e.CallbackQuery.Message;
                var chatId = message.Chat.Id;
                var button = JsonConvert
                    .DeserializeObject<ButtonInfo>(e.CallbackQuery.Data);
                bool correct = button.Answer == GetAnswer(button.TaskKey);

                CheckMessageSolved(message, correct, button.Answer);
                Console.WriteLine($"Sent in {timer.ElapsedMilliseconds}ms.");
                
                if (CanIncreaseLevel(chatId, correct))
                    SendMessage(chatId,
                        "Good job! You can now raise the difficulty, if you want");
                database.SetSolved(chatId, button.TaskKey, correct);
                timer.Stop();
                Console.WriteLine($"Uploaded in {timer.ElapsedMilliseconds}ms.");
            });
        }

        private async void MessageHandler(object sender, MessageEventArgs e)
        {
            await Task.Factory.StartNew(() =>
            {
                var timer = Stopwatch.StartNew();
                var message = e.Message.Text;
                var chatId = e.Message.Chat.Id;
                if (commands.ContainsKey(message))
                    commands[message](chatId);
                else
                    SendMessage(chatId, "Sorry, I didn't catch that");
                timer.Stop();
                Console.WriteLine($"Sent in {timer.ElapsedMilliseconds}ms.");
            });
        }
        #endregion

        private void IncreaseLevel(long userId)
        {
            var level = GetLevel(userId);
            database.SetLevel(userId, level + 1);
            var key = database.AddTask(userId, -1, new Exercise());
            database.SetSolved(userId, key, false);
        }

        private InlineKeyboardMarkup GetReplyMarkup(Exercise exercise, string taskKey)
        {
            var buttons = exercise
                .GetOptions(random, 4)
                .Select(option =>
                    new InlineKeyboardCallbackButton(
                        option,
                        JsonConvert.SerializeObject(
                            new ButtonInfo(option, taskKey))))
                .ToArray<InlineKeyboardButton>();
            return new InlineKeyboardMarkup(new[]
                {
                    new[] {buttons[0], buttons[1]},
                    new[] {buttons[2], buttons[3]}
                }
            );
        }

        private bool CanIncreaseLevel(long userId, bool? lastSolved=null)
        {
            var level = GetLevel(userId);
            var stats = database.GetLastStats(userId).ToList();
            if (lastSolved != null)
                stats[stats.Count - 1] = lastSolved;
            return level + 1 < levels.Length &&
                   levels[level].IsFinished(stats, userId);
        }

        #region SendOrEdit
        private void CheckMessageSolved(Message message, bool correct, string answer)
        {
            var builder = new StringBuilder(message.Text);
            builder.Insert(0, "```\n");
            builder.Append("```\n\n");
            builder.Append(answer);
            builder.Append(" - ");
            builder.Append(correct ? "Correct" : "Wrong answer");
            bot.EditMessageTextAsync(
                message.Chat.Id,
                message.MessageId,
                builder.ToString(),
                ParseMode.Markdown);
        }

        private void SendNewTask(long userId)
        {
            var level = GetLevel(userId);
            var exercise = levels[level].Generate(random);
            var taskKey = database.AddTask(userId, level, exercise);
            answersCache[taskKey] = exercise.Complexity.Value;
            bot.SendTextMessageAsync(
                userId,
                $"```\nFind the complexity of the algorithm:\n\n{exercise.Message}\n```",
                ParseMode.Markdown,
                replyMarkup: GetReplyMarkup(exercise, taskKey));
        }

        private void SendMessage(long userId, string message)
        {
            var buttons = new List<string> {"Give a task"};
            if (CanIncreaseLevel(userId))
                buttons.Add("I want harder");
            bot.SendTextMessageAsync(
                userId,
                message,
                replyMarkup: new ReplyKeyboardMarkup(new[]
                    {
                        buttons
                            .Select(str => new KeyboardButton(str))
                            .ToArray()
                    },
                    true));
        }
        #endregion
        
        #region Cache
        private string GetAnswer(string key)
        {
            if (answersCache.ContainsKey(key))
            {
                var answer = answersCache[key];
                answersCache.Remove(key);
                return answer;
            }

            return database.GetAnswer(key);
        }

        private int GetLevel(long chatId)
        {
            if (levelCache.ContainsKey(chatId))
                return levelCache[chatId];
            return levelCache[chatId] = database.GetLevel(chatId) ?? 0;
        }
        #endregion
    }
}