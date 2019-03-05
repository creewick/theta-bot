using System.Configuration;
using CommandLine;

namespace Theta_Bot
{
    public class Options
    {
        [Option('t', HelpText = "Telegram API token")]
        public string TelegramToken { get; set; }

        [Option("da", HelpText = "Address of database")]
        public string DatabaseAddress { get; set; }

        [Option("dt", HelpText = "Database token")]
        public string DatabaseToken { get; set; }

        public static Options FromConfig => new Options
        {
            TelegramToken = ConfigurationManager.AppSettings["telegramToken"],
            DatabaseAddress = ConfigurationManager.AppSettings["databaseAddress"],
            DatabaseToken = ConfigurationManager.AppSettings["databaseToken"]
        };
    }
}
