using System;

namespace Theta_Bot.Clients
{
    public interface IClient
    {
        void Start();
        event Action<int, string> OnMessage;
        event Action<int, string> OnButton;
        void SendText(int userId, string message);
    }
}