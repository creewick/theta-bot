using Telegram.Bot;

namespace theta_bot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new ThetaBot(
                new TelegramBotClient(args[0]),
                new SQLiteDataProvider(args[1]), 
                new ILevel[]
                {
                    new Level0(),
                    new Level1(),
                    new Level2(), 
                });
        }
    }
}