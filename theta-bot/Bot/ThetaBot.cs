using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using theta_bot.Database;
using theta_bot.Logic;
using theta_bot.Logic.Levels;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineKeyboardButtons;
using Telegram.Bot.Types.ReplyMarkups;

namespace theta_bot
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ThetaBot
    {
        // ReSharper disable InconsistentNaming
        private readonly Dictionary<string, Action<Message>> commands;
        private readonly Dictionary<long, int> keepLevelCache;
        private readonly Cache<string, string> answerCache;
        private readonly Cache<long, int?> levelCache;
       
        private static readonly Random random = new Random();
        private readonly IDataProvider database;
        private readonly Timer keepAliveTimer;
        private readonly TelegramBotClient bot;
        private readonly ILevel[] levels;

        public ThetaBot(TelegramBotClient bot, IDataProvider database, params ILevel[] levels)
        {           
            answerCache = new Cache<string, string>(database.GetAnswer, true);
            levelCache = new Cache<long, int?>(database.GetLevel, false);
            keepLevelCache = new Dictionary<long, int>();
            
            commands = new Dictionary<string, Action<Message>>
            {
                ["nexttask"] = DeleteButtonSendTask,
                ["levelup"] = IncreaseLevelSendTask
            };
            
            this.database = database;
            this.levels = levels;
            this.bot = bot;
            
            keepAliveTimer = new Timer(KeepAlive, null, 0, 10 * 60 * 1000);
            
            bot.OnCallbackQuery += ButtonHandler;
            bot.OnMessage += MessageHandler;

            bot.StartReceiving();
            Console.ReadLine();
            bot.StopReceiving();
        }

        private void KeepAlive(object e)
        {
            Console.WriteLine("keep alive sent");
        }
        
        private void ButtonHandler(object sender, CallbackQueryEventArgs e)
        {
            var buttonData = e.CallbackQuery.Data;
            var message = e.CallbackQuery.Message;

            Task.Run(() => 
            {   
                if (commands.ContainsKey(buttonData))
                    commands[buttonData](message);
                else
                    CheckTask(message, buttonData);
            });
        }

        private void MessageHandler(object sender, MessageEventArgs e)
        {
            Console.WriteLine($"Recieve message from {e.Message.Chat.Id}");
            Task.Run(() => { SendNewTask(e.Message.Chat.Id); });
        }
        
        private void CheckTask(Message message, string data)
        {
            var chatId = message.Chat.Id;
            var button = JsonConvert.DeserializeObject<ButtonInfo>(data);
            bool correct = button.Answer == answerCache.Get(button.TaskKey);

            MarkMessageSolved(message, correct, button.Answer);
            database.SetSolved(chatId, button.TaskKey, correct);

            if (!correct) return;
            
            if (CanIncreaseLevel(chatId))
            {
                keepLevelCache[chatId] = 5;
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
        
        private void IncreaseLevelSendTask(Message message)
        {
            var chatId = message.Chat.Id;
            var level = levelCache.Get(chatId) ?? 0;
            RemoveKeyboard(message);
            bot.SendTextMessageAsync(chatId,
                "Level up!");
            levelCache.Set(chatId, level + 1);
            database.SetLevel(chatId, level + 1);
            SendNewTask(chatId);
        }

        private void RemoveKeyboard(Message message)
        {
            bot.EditMessageReplyMarkupAsync(
                message.Chat.Id,
                message.MessageId);
        }

        private static InlineKeyboardMarkup GetKeyboard(Exercise exercise, string taskKey)
        {
            var buttons = exercise
                .GenerateOptions(random, 4)
                .Select(option =>
                    new InlineKeyboardCallbackButton(
                        option.ToString(),
                        JsonConvert.SerializeObject(new ButtonInfo(
                            option.ToString(), 
                            taskKey))))
                .ToArray();
            return new InlineKeyboardMarkup(new[]
                {
                    new[] {buttons[0], buttons[1]},
                    new[] {buttons[2], buttons[3]}
                }
            );
        }

        private bool CanIncreaseLevel(long userId)
        {
            if (keepLevelCache.ContainsKey(userId))
                if (keepLevelCache[userId]-- == 0)
                    keepLevelCache.Remove(userId);
                else
                    return false;
            var level = levelCache.Get(userId) ?? 0;
            var stats = database.GetLastStats(userId, 10).ToList();
            stats[stats.Count - 1] = true;
            return level + 1 < levels.Length &&
                   levels[level].IsFinished(stats, userId);
        }

        private void MarkMessageSolved(Message message, bool correct, string answer)
        {
            var result = correct ? "Correct" : "Wrong answer";
            bot.EditMessageTextAsync(
                message.Chat.Id,
                message.MessageId,
                $"```\n{message.Text}```\n\n{answer} - {result}",
                ParseMode.Markdown,
                replyMarkup: correct
                    ? null 
                    : new InlineKeyboardMarkup(new[]
                        {new InlineKeyboardCallbackButton("Next task", "nexttask")}));
        }

        private void DeleteButtonSendTask(Message message)
        {
            RemoveKeyboard(message);
            SendNewTask(message.Chat.Id);
        }

        private void SendNewTask(long userId)
        {
            var level = levelCache.Get(userId) ?? 0;
            var exercise = levels[level].Generate(random);
            var taskKey = database.AddTask(userId, level, exercise);
            answerCache.Set(taskKey, exercise.GetComplexity().ToString());
            bot.SendTextMessageAsync(
                userId,
                $"```\nFind the complexity of the algorithm:\n\n{exercise.GetCode(random)}\n```",
                ParseMode.Markdown,
                replyMarkup: GetKeyboard(exercise, taskKey));
        }
    }
}