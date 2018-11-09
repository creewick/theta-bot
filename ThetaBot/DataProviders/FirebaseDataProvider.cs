using System;
using System.Threading.Tasks;
using DataProviders.Models;
using Firebase.Database;
using Firebase.Database.Query;

namespace DataProviders
{
    public class FireBaseDataProvider : IDataProvider
    {
        private readonly FirebaseClient client;

        public FireBaseDataProvider(string databaseUrl, string databaseSecret)
        {
            client = new FirebaseClient(databaseUrl,
                new FirebaseOptions{AuthTokenAsyncFactory = () =>
                    Task.FromResult(databaseSecret)});
        }

        public async Task<string> GetIdForNewTask(string userId, TaskInfo task)
        {
            var id = FirebaseKeyGenerator.Next();
            await client
                .Child("Tasks")
                .Child(id)
                .PutAsync(task);
            return id;
        }

        public async Task<string> GetAnswerForTask(string taskKey)
        {
            return await client
                .Child("Tasks")
                .Child(taskKey)
                .Child("Answer")
                .OnceSingleAsync<string>();
        }

        public async Task SetTaskStatus(string userId, string taskKey, bool isCorrect)
        {
            var time = DateTime.UtcNow;
            var data = new TaskInfo{IsCorrectAnswer = isCorrect};
            await client
                .Child("Tasks")
                .Child(taskKey)
                .PatchAsync(data);
        }

        public async Task SetUserLevel(string userId, LevelInfo level)
        {
            await client
                .Child("Users")
                .Child(userId)
                .PatchAsync(level);
        }

        public async Task<LevelInfo> GetUserLevel(string userId)
        {
            return await client
                .Child("Users")
                .Child(userId)
                .OnceSingleAsync<LevelInfo>();
        }
    }
}
