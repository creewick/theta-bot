using System.Threading.Tasks;
using NLog;
using Theta_Bot.Clients;
using Theta_Bot.Database;

namespace Theta_Bot.Logic
{
    public class ThetaBot
    {
        private readonly IDatabase database;
        private readonly IClient[] clients;
        private readonly Logger log;
        private readonly UI ui;
        public ThetaBot(IDatabase database, IClient[] clients)
        {
            log = LogManager.GetCurrentClassLogger();
            ui = new UI(database, clients);
            this.database = database;
            this.clients = clients;

            foreach (var client in clients)
            {
                client.OnMessage += (userId, message) => OnMessage(client, userId, message);
                client.OnButton += (userId, data) => OnButton(client, userId, data);
            }
        }

        public void Start()
        {
            foreach (var client in clients)
                client.Start();
        }
        
        private async Task OnMessage(IClient client, string userId, string message)
        {
            log.Debug($"User [{userId}] sent a message [{message}]");
            
            var level = await database
                .GetCurrentLevelAsync(userId);

            if (level == null)
                await ui.SendAvailableLevelsList(userId);
        }
        
        private void OnButton(IClient client, string userId, string data)
        {
            log.Debug($"User [{userId}] pressed a button [{data}]");
        }
    }
}
