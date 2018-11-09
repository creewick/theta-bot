using System;
using Telegram.Bot;

namespace TelegramClient
{
    public class TelegramClient
    {
        private readonly TelegramBotClient bot;
        private readonly ThetaBot.ThetaBot theta;

        public TelegramClient(TelegramBotClient bot, ThetaBot.ThetaBot theta)
        {
            this.bot = bot;
            this.theta = theta;


            bot.OnCallbackQuery += ButtonHandler;
            bot.OnMessage += MessageHandler;
        }

        public void Run()
        {

            bot.StartReceiving();
            Console.ReadLine();
            bot.StopReceiving();
        }
    }
}
