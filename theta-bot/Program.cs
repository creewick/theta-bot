using System;
using System.Net;
using CommandLine;
using Ninject;
using theta_bot.Classes;
using theta_bot.Database;
using theta_bot.NewGenerators;
using Telegram.Bot;

namespace theta_bot
{
    public static class Program
    {
        public static void Main(string[] args)
        {
//            Parser.Default.ParseArguments<Options>(args)
//                .WithParsed(Resolve);
        }

//        private static void Resolve(Options options)
//        {
//            var di = new StandardKernel();
//            
//            if (options.Proxy == null)
//                di.Bind<TelegramBotClient>()
//                    .ToMethod(c => new TelegramBotClient(options.TelegramApiToken)); 
//            else
//                di.Bind<TelegramBotClient>()                  
//                    .ToConstructor(c => 
//                        new TelegramBotClient(c.Inject<string>(), c.Inject<WebProxy>()))
//                    .WithConstructorArgument("token", options.TelegramApiToken)
//                    .WithConstructorArgument("webProxy", new WebProxy(options.Proxy));
//            
//            if (options.DatabaseToken == null)
//                di.Bind<IDataProvider>()
//                    .To<SqLiteProvider>()
//                    .WithConstructorArgument("filename", options.DatabaseAddress);
//            else
//                di.Bind<IDataProvider>()
//                    .To<FirebaseProvider>()
//                    .WithConstructorArgument("url", options.DatabaseAddress)
//                    .WithConstructorArgument("token", options.DatabaseToken);
//            
//            di.Bind<ThetaBot>()
//                .To<ThetaBot>()
//                .WithConstructorArgument("levels", new ILevel[]
//                    {new Level0(), new Level1(), new Level2()});
//
//            di.Get<ThetaBot>();
//        }
    }
}