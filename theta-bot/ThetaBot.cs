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
        private readonly Dictionary<string, string> answersCache = new Dictionary<string, string>();
        private readonly Dictionary<long, int> levelCache = new Dictionary<long, int>();
        
        private readonly Dictionary<string, Action<Message>> queries = new Dictionary<string, Action<Message>>();
        
        private readonly Random random = new Random();
        private readonly IDataProvider database;
        private readonly TelegramBotClient bot;
        private readonly ILevel[] levels;

        public ThetaBot(TelegramBotClient bot, IDataProvider database, params ILevel[] levels)
        {
            this.bot = bot;
            this.database = database;
            this.levels = levels;

            bot.OnMessage += MessageHandler;
            bot.OnCallbackQuery += AnswerHandler;
            
            queries.Add("nexttask", DeleteButtonSendTask);
            queries.Add("levelup", IncreaseLevel);

            bot.StartReceiving();
            Console.ReadLine();
            bot.StopReceiving();
        }

        #region Handlers
        private void AnswerHandler(object sender, CallbackQueryEventArgs e)
        {
            Task.Run(() => 
            {   
                var timer = Stopwatch.StartNew();
                var data = e.CallbackQuery.Data;
                if (queries.ContainsKey(data))
                    queries[data](e.CallbackQuery.Message);
                else
                    CheckTask(e.CallbackQuery.Message, data);
                timer.Stop();
                Console.WriteLine(timer.ElapsedMilliseconds);
            });
        }

        private void MessageHandler(object sender, MessageEventArgs e)
        {
            Task.Run(() => { SendNewTask(e.Message.Chat.Id); });
        }
        #endregion
        
        private void CheckTask(Message message, string data)
        {
            var chatId = message.Chat.Id;
            var button = JsonConvert
                .DeserializeObject<ButtonInfo>(data);
            bool correct = button.Answer == GetAnswer(button.TaskKey);

            MarkMessageSolved(message, correct, button.Answer);
            database.SetSolved(chatId, button.TaskKey, correct);

            if (!correct) return;
            
            if (CanIncreaseLevel(chatId, correct))
            {
                var key = database.AddTask(chatId, -1, new Exercise());
                database.SetSolved(chatId, key, false);
                bot.SendTextMessageAsync(chatId,
                    "Good job! Do you want to raise the difficulty?",
                    replyMarkup: new InlineKeyboardMarkup(new[]
                    {
                        new InlineKeyboardCallbackButton("Yes", "levelup"),
                        new InlineKeyboardCallbackButton("No, later", "nexttask")
                    }));
            }
            else
                Task.Run(()=>SendNewTask(chatId));

        }
        
        private void IncreaseLevel(Message message)
        {
            var chatId = message.Chat.Id;
            var level = GetLevel(chatId);
            bot.EditMessageReplyMarkupAsync(
                chatId,
                message.MessageId);
            bot.SendTextMessageAsync(chatId,
                "Level up!");
            levelCache[chatId] = level + 1;
            database.SetLevel(chatId, level + 1);
            SendNewTask(chatId);
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
            var stats = database.GetLastStats(userId, 10).ToList();
            if (lastSolved != null)
                stats[stats.Count - 1] = lastSolved;
            return level + 1 < levels.Length &&
                   levels[level].IsFinished(stats, userId);
        }

        #region SendOrEdit
        private void MarkMessageSolved(Message message, bool correct, string answer)
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
                ParseMode.Markdown,
                replyMarkup: correct
                    ? null 
                    : new InlineKeyboardMarkup(new[]
                        {new InlineKeyboardCallbackButton("Next task", "nexttask")}));
        }

        private void DeleteButtonSendTask(Message message)
        {
            bot.EditMessageReplyMarkupAsync(
                message.Chat.Id,
                message.MessageId);
            SendNewTask(message.Chat.Id);
        }

        private void SendNewTask(long userId)
        {
            bot.SendTextMessageAsync(userId, "test");
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