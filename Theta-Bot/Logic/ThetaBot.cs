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
        public ThetaBot(IDatabase database, IClient[] clients)
        {
            log = LogManager.GetCurrentClassLogger();
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
        
        private void OnMessage(IClient client, int userId, string message)
        {
            log.Debug($"User [{userId}] sent a message [{message}]");
        }
        
        private void OnButton(IClient client, int userId, string data)
        {
            log.Debug($"User [{userId}] pressed a button [{data}]");
        }
    }
}
