using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;

namespace Theta_Bot.Database
{
    public class FirebaseDatabase : IDatabase
    {
        private readonly FirebaseClient client;

        public FirebaseDatabase(string url, string secret)
        {
            client = new FirebaseClient(url, new FirebaseOptions
                {AuthTokenAsyncFactory = () => Task.FromResult(secret)});
        }

        public async Task<List<string>> GetCompletedLevelsAsync(string userId)
        {
            return (await client
                .Child("users")
                .Child(userId)
                .Child("completed-levels")
                .OnceSingleAsync<Dictionary<string, string>>()
                )
                .Select(x => x.Value)
                .ToList();
        }

        public async Task SetLevelCompletedAsync(string userId, string levelId)
        {
            await client
                .Child("users")
                .Child(userId)
                .Child("completed-levels")
                .PostAsync(levelId);
        }

        public async Task<string> GetCurrentLevelAsync(string userId)
        {
            return await client
                .Child("users")
                .Child(userId)
                .Child("currentLevel")
                .Child("levelId")
                .OnceSingleAsync<string>();
        }

        public async Task SetCurrentLevelAsync(string userId, string levelId)
        {
            await client
                .Child("userProgress")
                .Child(userId)
                .Child("currentLevel")
                .Child("levelId")
                .PutAsync(levelId);
        }
    }
}
