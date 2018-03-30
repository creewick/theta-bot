using Telegram.Bot;

namespace theta_bot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new ThetaBot(
                new TelegramBotClient(args[0]),
                new DataProvider(args[1]), 
                new[]
                {
                    new Level0()
                });
        }
    }
}