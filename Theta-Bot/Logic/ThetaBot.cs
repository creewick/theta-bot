using Theta_Bot.Database;

namespace Theta_Bot.Logic
{
    public class ThetaBot
    {
        private readonly IDataProvider database;

        public ThetaBot(IDataProvider database)
        {
            this.database = database;
        }
    }
}
