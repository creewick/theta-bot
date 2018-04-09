using Telegram.Bot;

namespace theta_bot
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            new FirebaseProvider("https://thetabot-4a6fa.firebaseio.com", "AIzaSyAIZ4KTPkWY2ZkIxOVavWCpakf82o7OcZ0");
//            new ThetaBot(
//                new TelegramBotClient(args[0]),
//                new SQLiteProvider(args[1]), 
//                new Level0(),
//                new Level1(),
//                new Level2() 
//                );
        }
    }
}