using System;
using System.Collections.Generic;
using System.Linq;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace theta_bot
{
    public class ThetaBot
    {
        private readonly TelegramBotClient bot;
        private readonly ILevel[] levels;
        private readonly Random random = new Random();
        
        public ThetaBot(string token, ILevel[] levels)
        {
            bot = new TelegramBotClient(token);
            this.levels = levels;
            
            bot.OnMessage += OnMessageReceive;
            
            bot.StartReceiving();
            Console.ReadLine();
            bot.StopReceiving();
        }

        private async void OnMessageReceive(object sender, MessageEventArgs args)
        {
            var message = args.Message;
            var exercise = GetExercise(message.Contact);
            
            await bot.SendTextMessageAsync(
                message.Chat.Id, 
                exercise.GetMessage(), 
                ParseMode.Markdown, 
                false, false, 0, 
                GetKeyboard(exercise.GetOptions(random, 4)));
        }
                
        private Exercise GetExercise(Contact person)
        {
            // TODO: Узнать уровень игрока
            return levels[0].Generate(random).BoundVars();
        }
        
        private static ReplyKeyboardMarkup GetKeyboard(IEnumerable<string> labels)
        {
            var keyboard = labels
                .Select(option => new KeyboardButton(option))
                .ToArray();
            return new ReplyKeyboardMarkup(keyboard, true, true);
        }
    }
}