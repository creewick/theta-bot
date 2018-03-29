﻿using System;
using System.Linq;
using System.Text;
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
            var builder = new StringBuilder(e.CallbackQuery.Message.Text);
            builder.Insert(0, "```\n");
            builder.Append("```\n\n");
            bot.EditMessageTextAsync(
                e.CallbackQuery.Message.Chat.Id,
                e.CallbackQuery.Message.MessageId,
                "True" == e.CallbackQuery.Data
                    ? builder.Append("Верно").ToString()
                    : builder.Append("Ответ неверный").ToString(),
                ParseMode.Markdown);
        }

        private async void OnMessageReceive(object sender, MessageEventArgs args)
        {
            var message = args.Message;
            switch (message.Text)
            {
                case "Дай задачу":
                    var exercise = GetExercise(message.Chat.Id);
                    data.StoreAnswer(message.Chat.Id, exercise.Complexity.Value);
                    await bot.SendTextMessageAsync(
                        message.Chat.Id, 
                        exercise.GetMessage(), 
                        ParseMode.Markdown, 
                        false, false, 0, 
                        GetInlineKeyboard(exercise));
                    break;
                case "Слишком просто":
                    await bot.SendTextMessageAsync(
                        message.Chat.Id, 
                        "Нет, это ты хорошо решаешь.");
                    break;
                default:
                    await bot.SendTextMessageAsync(
                        message.Chat.Id,
                        "Привет. Нажми на кнопку, чтобы получить задачу",
                        ParseMode.Default,
                        false, false, 0,
                        new ReplyKeyboardMarkup(new[]{
                            new[] {new KeyboardButton("Дай задачу")},
                            new[] {new KeyboardButton("Слишком просто")}}, true));
                    break;
            }
        }
        
        private Exercise GetExercise(long chatId)
        {
            // TODO: Узнать уровень игрока
            return levels[0].Generate(random);
        }

        private InlineKeyboardMarkup GetInlineKeyboard(Exercise exercise) => 
            new InlineKeyboardMarkup(
                exercise
                    .GetOptions(random, 4)
                    .Select(option => new InlineKeyboardCallbackButton(
                            option, 
                           (option == exercise.Complexity.Value).ToString()))
                    .ToArray<InlineKeyboardButton>());
    }
}