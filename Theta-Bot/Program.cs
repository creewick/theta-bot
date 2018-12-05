using CommandLine;
using Ninject;
using Theta_Bot.Database;
using Theta_Bot.Logic;
using Theta_Bot.Telegram;

namespace Theta_Bot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Parser.Default
                .ParseArguments<Options>(args)
                .WithParsed(Start)
                .WithNotParsed(x => Start(Options.FromConfig));
        }

        private static void Start(Options options)
        {
            var container = new StandardKernel();

            container
                .Bind<IDataProvider>()
                .ToMethod(c => new FirebaseDataProvider(
                    options.DatabaseAddress,
                    options.DatabaseToken));

            var thetaBot = container.Get<ThetaBot>();
            var telegramClient = new TelegramClient(thetaBot, options.TelegramToken);
            
            telegramClient.Start();
        }
    }
}
