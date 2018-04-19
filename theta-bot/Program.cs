using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
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
            var di = new StandardKernel();
            di.Bind<TelegramBotClient>()
                .ToConstructor(c => new TelegramBotClient(c.Inject<string>(), c.Inject<WebProxy>()))
                .WithConstructorArgument("token", options.TelegramApiToken)
                .WithConstructorArgument("webProxy", new WebProxy("188.230.99.59", 3128));
//            di.Bind<IDataProvider>()
//                .To<FirebaseProvider>()
//                .WithConstructorArgument("url", options.DatabaseAddress)
//                .WithConstructorArgument("token", options.DatabaseToken);
            di.Bind<IDataProvider>()
                .To<SQLiteProvider>()
                .WithConstructorArgument("filename", options.DatabaseAddress);
            di.Bind<ThetaBot>()
                .To<ThetaBot>()
                .WithConstructorArgument("levels", new ILevel[] 
                    {new Level0(), new Level1(), new Level2()});

            di.Get<ThetaBot>();
        }
    }
}