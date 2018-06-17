using System;
using System.Configuration;
using System.Net;
using CommandLine;
using Ninject;
using theta_bot.Database;
using theta_bot.Logic.Levels;
using Telegram.Bot;

namespace theta_bot
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(Resolve);
        }

        private static void Resolve(Options options)
        {
            options = AppendFromConfig(options);
            var di = new StandardKernel();
            
            if (options.Proxy == null)
                di.Bind<TelegramBotClient>()
                    .ToMethod(c => new TelegramBotClient(options.TelegramApiToken)); 
            else
                di.Bind<TelegramBotClient>()                  
                    .ToConstructor(c => new TelegramBotClient
                        (c.Inject<string>(), c.Inject<WebProxy>()))
                    .WithConstructorArgument("token", options.TelegramApiToken)
                    .WithConstructorArgument("webProxy", new WebProxy(options.Proxy));
            
            if (options.DatabaseToken == null)
                di.Bind<IDataProvider>()
                    .To<SqLiteProvider>()
                    .WithConstructorArgument("filename", options.DatabaseAddress);
            else
                di.Bind<IDataProvider>()
                    .To<FirebaseProvider>()
                    .WithConstructorArgument("url", options.DatabaseAddress)
                    .WithConstructorArgument("token", options.DatabaseToken);
            
            di.Bind<ThetaBot>()
                .To<ThetaBot>()
                .WithConstructorArgument("levels", new ILevel[]
                    {new Level0(), new Level1()});

            di.Get<ThetaBot>();
        }

        private static Options AppendFromConfig(Options options)
        {
            var telegramToken = ConfigurationManager.AppSettings["telegramToken"];
            if (telegramToken.Length > 0)
                options.TelegramApiToken = telegramToken;
            var proxy = ConfigurationManager.AppSettings["proxy"];
            if (proxy.Length > 0)
                options.Proxy = proxy;
            var databaseAddress = ConfigurationManager.AppSettings["databaseAddress"];
            if (databaseAddress.Length > 0)
                options.DatabaseAddress = databaseAddress;
            var databaseToken = ConfigurationManager.AppSettings["databaseToken"];
            if (databaseToken.Length > 0)
                options.DatabaseToken = databaseToken;
            return options;
        }
    }
}