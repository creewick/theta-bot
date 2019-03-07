using CommandLine;
using Ninject;
using NLog;
using Theta_Bot.Clients;
using Theta_Bot.Database;

namespace Theta_Bot
{
    public static class Program
    {
        private static readonly StandardKernel Container = new StandardKernel();
        public static void Main(string[] args)
        {
            ConfigureLog();
            
            Parser.Default
                .ParseArguments<Options>(args)
                .WithParsed(Start)
                .WithNotParsed(x => Start(Options.FromConfig));
        }

        private static void Start(Options options)
        {
            Container
                .Bind<IDatabase>()
                .To<FirebaseDatabase>()
                .WithConstructorArgument("url", options.DatabaseAddress)
                .WithConstructorArgument("secret", options.DatabaseToken);
            Container
                .Bind<IClient>()
                .To<TelegramClient>()
                .WithConstructorArgument("token", options.TelegramToken);

            var a = Container
                .Get<IDatabase>()
                .GetCompletedLevelsAsync("1")
                .Result;

//            Container
//                .Get<ThetaBot>()
//                .Start();
        }

        private static void ConfigureLog()
        {
            var config = new NLog.Config.LoggingConfiguration();
            var logFile = new NLog.Targets.FileTarget("logFile"){ FileName = "log" };
            var logConsole = new NLog.Targets.ConsoleTarget("logConsole");
            
            config.AddRule(LogLevel.Warn, LogLevel.Fatal, logFile);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logConsole);

            LogManager.Configuration = config;
        }
    }
}
