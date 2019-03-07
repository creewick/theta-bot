using System;
using MihaZupan;
using NLog;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Theta_Bot.Clients
{
    public class TelegramClient : IClient
    {
        public event Action<string, string> OnMessage;
        public event Action<string, string> OnButton;
        private readonly TelegramBotClient client;

        private readonly Logger log;
        public TelegramClient(string token)
        {
            log = LogManager.GetCurrentClassLogger();
            
            var proxy = new HttpToSocks5Proxy(
                "proxy.freemedia.io", 
                1337, 
                "freemedia", 
                "freemedia");

            client = new TelegramBotClient(token, proxy);

            client.OnMessage += (_, e) => OnMessage(e.Message.From.Id.ToString(), e.Message.Text);
            client.OnCallbackQuery += (_, e) => OnButton(e.CallbackQuery.From.Id.ToString(), e.CallbackQuery.Data);
        }

        public void Start()
        {
            client.StartReceiving();
            Console.ReadLine();
            client.StopReceiving();
        }

        public void SendText(string userId, string message)
        {
            client.SendTextMessageAsync(userId, message, ParseMode.Markdown);
            log.Debug($"Message [{message}] sent to user [{userId}]");
        }
    }
}
