using System;
using System.Collections.Generic;
using System.Net;
using CommandLine;
using Ninject;
using Ninject.Parameters;
using Telegram.Bot;

namespace theta_bot
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(Resolve);
        }

        private static void Resolve(Options options)
        {
            new ThetaBot(
                new TelegramBotClient(options.TelegramApiToken, new WebProxy("198.27.66.158", 1080)),
                new SQLiteProvider("database.db"),
                //new FirebaseProvider(args[1], args[2]), 
                new Level0(),
                new Level1(),
                new Level2() 
            );
//            var di = new StandardKernel();
//            di.Bind<TelegramBotClient>()
//                .ToMethod(c => new TelegramBotClient(
//                    options.TelegramApiToken,
//                    new WebProxy("138.197.157.66", 1080)));
//            di.Bind<IDataProvider>()
//                .To<SQLiteProvider>()
//                .WithConstructorArgument("filename", options.DatabaseAddress);
//            di.Bind<ThetaBot>()
//                .To<ThetaBot>()
//                .WithConstructorArgument("levels", new ILevel[] {new Level0(), new Level1(), new Level2()});
//
//            di.Get<ThetaBot>();
        }
    }
}