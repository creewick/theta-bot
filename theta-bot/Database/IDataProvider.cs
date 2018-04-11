using System.Collections.Generic;

namespace theta_bot
{
    public interface IDataProvider
    {
        string AddTask(long chatId, int level, Exercise exercise);
        string GetAnswer(string taskKey);
        void SetSolved(long chatId, string taskKey, bool solved);
        IEnumerable<bool?> GetLastStats(long chatId);
        void SetLevel(long chatId, int level);
        int? GetLevel(long chatId);
    }
}