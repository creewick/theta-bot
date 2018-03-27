using theta_bot.Levels;

namespace theta_bot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            const string token = "452741789:AAHCMogCOadheB55ZD84k0zLWaD4nyB62c0";
            new ThetaBot(token,
                new[]
                {
                    new Level1()
                });
        }
    }
}