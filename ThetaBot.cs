using System;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace theta_bot
{
    public class ThetaBot
    {
        private readonly TelegramBotClient Bot;

        public ThetaBot(string token)
        {
            Bot = new TelegramBotClient(token);

            Bot.OnMessage += OnMessageReceive;
            
            Bot.StartReceiving();
            Console.ReadLine();
            Bot.StopReceiving();
        }

        private async void OnMessageReceive(object sender, MessageEventArgs args)
        {
            var message = args.Message;
            await Bot.SendTextMessageAsync(message.Chat.Id, "42");
        }
    }
}