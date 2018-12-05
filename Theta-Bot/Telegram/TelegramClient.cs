using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Theta_Bot.Logic;

namespace Theta_Bot.Telegram
{
    public class TelegramClient
    {
        private readonly TelegramBotClient client;
        private readonly ThetaBot bot;

        public TelegramClient(ThetaBot bot, string token)
        {
            client = new TelegramBotClient(token);
            this.bot = bot;

            client.OnCallbackQuery += ButtonHandler;
            client.OnMessage += MessageHandler;
        }

        public void Start()
        {
            client.StartReceiving();
            Console.ReadLine();
            client.StopReceiving();
        }

        private void MessageHandler(object _, MessageEventArgs e)
        {
            client.SendTextMessageAsync(
                chatId: e.Message.Chat.Id,
                text: "Hello");
        }

        private void ButtonHandler(object _, CallbackQueryEventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}
