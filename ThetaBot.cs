﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
 using Telegram.Bot.Types.InlineKeyboardButtons;
 using Telegram.Bot.Types.ReplyMarkups;

namespace theta_bot
{
    public class ThetaBot
    {
        private readonly Dictionary<long, int> levelsCache = new Dictionary<long, int>();
        private readonly List<long> toldAboutLevel = new List<long>();
        private readonly TelegramBotClient bot;
        private readonly IDataProvider data;
        private readonly ILevel[] levels;
        private readonly Random random = new Random();
        
        public ThetaBot(TelegramBotClient bot, IDataProvider data, ILevel[] levels)
        {
            this.bot = bot;            
            this.data = data;
            this.levels = levels;
            
            bot.OnMessage += OnMessageReceive;
            bot.OnCallbackQuery += CheckAnswer;
            
            bot.StartReceiving();
            Console.ReadLine();
            bot.StopReceiving();
        }

        private void CheckAnswer(object sender, CallbackQueryEventArgs e)
        {
            var buttonInfo = JsonConvert
                .DeserializeObject<Dictionary<string, string>>(e.CallbackQuery.Data);
            var correctAnswer = data.GetAnswer(int.Parse(buttonInfo["taskId"]));
            var isCorrect = buttonInfo["button"] == correctAnswer;
            
            data.SetSolved(
                int.Parse(buttonInfo["taskId"]), 
                isCorrect);
            CheckNextLevel(e.CallbackQuery.Message.Chat.Id);
            EditTaskMessage(e.CallbackQuery.Message, isCorrect, buttonInfo["button"]);
        }

        private void EditTaskMessage(Message message, bool correct, string answer)
        {
            var builder = new StringBuilder(message.Text);
            builder.Insert(0, "```\n");
            builder.Append("```\n\n");
            builder.Append(answer);
            builder.Append(" - ");
            bot.EditMessageTextAsync(
                message.Chat.Id,
                message.MessageId,
                correct
                    ? builder.Append("Верно").ToString()
                    : builder.Append("Ответ неверный").ToString(),
                ParseMode.Markdown); 
        }

        private async void CheckNextLevel(long chatId)
        {
            var level = GetLevel(chatId);
            if (levels[level].IsFinished(data, chatId) &&
                level + 1 < levels.Length &&
                !toldAboutLevel.Contains(chatId))
            {
                toldAboutLevel.Add(chatId);
                await bot.SendTextMessageAsync(
                    chatId,
                    "Отлично справляешься! Если хочешь, можешь взять задачи посложнее.",
                    ParseMode.Default,
                    false, false, 0,
                    new ReplyKeyboardMarkup(new[] { new[]
                        { new KeyboardButton("Дай задачу"),
                          new KeyboardButton("Хочу сложнее") }
                    }, true));
            }
        }
        
        private async void OnMessageReceive(object sender, MessageEventArgs args)
        {
            var message = args.Message;
            switch (message.Text)
            {
                case "Дай задачу":
                    var exercise = GetExercise(message.Chat.Id);
                    var taskId = data.AddTask(message.Chat.Id, exercise.Complexity.Value);
                    await bot.SendTextMessageAsync(
                        message.Chat.Id, 
                        exercise.GetMessage(), 
                        ParseMode.Markdown, 
                        false, false, 0, 
                        GetInlineKeyboard(exercise, taskId));
                    break;
                case "Хочу сложнее":
                    var level = GetLevel(message.Chat.Id);
                    var finished = levels[level].IsFinished(data, message.Chat.Id) && level+1 < levels.Length;
                    if (finished)
                    {
                        data.SetLevel(message.Chat.Id, level + 1);
                        levelsCache[message.Chat.Id]++;

                        var id = data.AddTask(message.Chat.Id, "");
                        data.SetSolved(id, false);
                    }

                    await bot.SendTextMessageAsync(
                        message.Chat.Id,
                        finished ? "Уровень повышен" : "Пока что повысить уровень нельзя",
                        ParseMode.Default,
                        false, false, 0,
                        new ReplyKeyboardMarkup(new[]{
                            new[] {new KeyboardButton("Дай задачу")}}, true));
                    break;
                default:
                    await bot.SendTextMessageAsync(
                        message.Chat.Id,
                        "Привет. Нажми на кнопку, чтобы получить задачу",
                        ParseMode.Default,
                        false, false, 0,
                        new ReplyKeyboardMarkup(new[]{
                            new[] {new KeyboardButton("Дай задачу")}}, true));
                    break;
            }
        }
        
        private int GetLevel(long chatId)
        {
            if (levelsCache.ContainsKey(chatId))
                return levelsCache[chatId];
            var level = data.GetLevel(chatId);
            if (level == -1)
            {
                data.SetLevel(chatId, 0);
                level = 0;
            }

            levelsCache[chatId] = level;
            return level;
        }
        
        private Task GetExercise(long chatId)
        {
            return levels[GetLevel(chatId)].Generate(random);
        }

        private InlineKeyboardMarkup GetInlineKeyboard(Task task, int taskId) => 
            new InlineKeyboardMarkup(
                task
                    .GetOptions(random, 4)
                    .Select(option => new InlineKeyboardCallbackButton(
                            option, 
                            JsonConvert.SerializeObject(
                                new Dictionary<string, string>
                                {
                                    {"taskId", taskId.ToString()},
                                    {"button", option}
                                }
                                )))
                    .ToArray<InlineKeyboardButton>());
    }
}