using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Bson;
using theta_bot.Levels;
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
            builder.Append("");
            bot.EditMessageTextAsync(
                e.CallbackQuery.Message.Chat.Id,
                e.CallbackQuery.Message.MessageId,
                "True" == e.CallbackQuery.Data
                    ? builder.Append("\tВерно").ToString()
                    : builder.Append("\tОтвет неверный").ToString(),
                ParseMode.Markdown);
        }

        private async void OnMessageReceive(object sender, MessageEventArgs args)
        {
            var message = args.Message;
            
            var exercise = GetExercise(message.Contact);
            data.StoreAnswer(message.Chat.Id, exercise.Complexity.Value);
            
            await bot.SendTextMessageAsync(
                message.Chat.Id, 
                exercise.GetMessage(), 
                ParseMode.Markdown, 
                false, false, 0, 
                GetInlineKeyboard(exercise));
        }
        
        private Exercise GetExercise(Contact person)
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

        private ReplyKeyboardMarkup GetKeyboard(IEnumerable<string> labels) => 
            new ReplyKeyboardMarkup(
                labels
                    .Select(option => new KeyboardButton(option))
                    .ToArray(), 
                true, true);
    }
}