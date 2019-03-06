using System;

namespace Theta_Bot.Clients
{
    public interface IClient
    {
        void Start();
        event Action<string, string> OnMessage;
        event Action<string, string> OnButton;
        void SendText(string userId, string message);
    }
}