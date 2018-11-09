using Models;
using System.Threading.Tasks;
using Task = Models.Task;

namespace DataProviders
{
    public interface IDataProvider
    {
        Task<string> GetIdForNewTask(string userId, Task task);
        Task<string> GetAnswerForTask(string taskKey);
        System.Threading.Tasks.Task SetTaskStatus(string userId, string taskKey, bool isCorrect);
        System.Threading.Tasks.Task SetUserProgress(string userId, Progress level);
        Task<Progress> GetUserProgress(string userId);
    }
}
