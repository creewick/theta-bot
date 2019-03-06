using System.Threading.Tasks;
using Theta_Bot.Clients;
using Theta_Bot.Database;

namespace Theta_Bot.Logic
{
    public class UI : ThetaBot
    {
        private readonly IDatabase database;
        private readonly IClient[] clients;

        public UI(IDatabase database, IClient[] clients) : base(database, clients)
        {
            this.database = database;
            this.clients = clients;
        }

        public async Task SendAvailableLevelsList(string userId)
        {
//            await database.
        }
    }
}