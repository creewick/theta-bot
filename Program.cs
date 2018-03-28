using theta_bot.Levels;

namespace theta_bot
{
    public class Program
    {
        private const string Token = "452741789:AAHCMogCOadheB55ZD84k0zLWaD4nyB62c0";

        public static void Main()
        {
            new ThetaBot(Token,
                new[]
                {
                    new Level1()
                },
                "database");
        }
    }
}