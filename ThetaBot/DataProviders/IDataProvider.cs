using System.Threading.Tasks;
using DataProviders.Models;

namespace DataProviders
{
    public interface IDataProvider
    {
        Task<string> GetIdForNewTask(string userId, TaskInfo task);
        Task<string> GetAnswerForTask(string taskKey);
        Task SetTaskStatus(string userId, string taskKey, bool isCorrect);
        Task SetUserLevel(string userId, LevelInfo level);
        Task<LevelInfo> GetUserLevel(string userId);
    }
}
