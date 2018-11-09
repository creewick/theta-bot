using System;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using Models;
using Task = Models.Task;

namespace DataProviders
{
    public class FireBaseDataProvider : IDataProvider
    {
        private readonly FirebaseClient client;

        public FireBaseDataProvider(string databaseUrl, string databaseSecret)
        {
            client = new FirebaseClient(databaseUrl,
                new FirebaseOptions{AuthTokenAsyncFactory = () =>
                    System.Threading.Tasks.Task.FromResult(databaseSecret)});
        }

        public async Task<string> GetIdForNewTask(string userId, Task task)
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

        public async System.Threading.Tasks.Task SetTaskStatus(string userId, string taskKey, bool isCorrect)
        {
            var time = DateTime.UtcNow;
            var data = new Task{IsCorrectAnswer = isCorrect};
            await client
                .Child("Tasks")
                .Child(taskKey)
                .PatchAsync(data);
        }

        public async System.Threading.Tasks.Task SetUserProgress(string userId, Progress level)
        {
            await client
                .Child("Users")
                .Child(userId)
                .PatchAsync(level);
        }

        public async Task<Progress> GetUserProgress(string userId)
        {
            return await client
                .Child("Users")
                .Child(userId)
                .OnceSingleAsync<Progress>();
        }
    }
}
