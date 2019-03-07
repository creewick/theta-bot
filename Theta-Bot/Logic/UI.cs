using System.Threading.Tasks;
using NLog;
using Theta_Bot.Clients;
using Theta_Bot.Database;

namespace Theta_Bot.Logic
{
    public class UI
    {
        private readonly IDatabase database;
        private readonly IClient[] clients;
        private readonly Logger log;

        public UI(IDatabase database, IClient[] clients)
        {
            log = LogManager.GetCurrentClassLogger();
            this.database = database;
            this.clients = clients;
        }

        public async Task SendAvailableLevelsList(string userId)
        {
            
        }
    }
}