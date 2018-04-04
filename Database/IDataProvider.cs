using System.Collections.Generic;

namespace theta_bot
{
    public interface IDataProvider
    {
        int AddTask(long chatId, Task task);
        string GetAnswer(int taskId);
        void SetSolved(int taskId, bool solved);
        IEnumerable<bool> GetLastStats(long chatId);
        void SetLevel(long chatId, int level);
        int GetLevel(long chatId);
    }
}