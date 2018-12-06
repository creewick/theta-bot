using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;

namespace Theta_Bot.Database
{
    public class FirebaseDataProvider : IDataProvider
    {
        private readonly FirebaseClient client;

        public FirebaseDataProvider(string url, string secret)
        {
            client = new FirebaseClient(url, new FirebaseOptions
                {AuthTokenAsyncFactory = () => Task.FromResult(secret)});
        }

        public async Task<Dictionary<string, bool>> GetCompletedLevelsAsync(string userId)
        {
            return await client
                .Child("userProgress")
                .Child(userId)
                .Child("completedLevels")
                .OnceSingleAsync<Dictionary<string, bool>>();
        }

        public async Task SetLevelAsCompletedAsync(string userId, string levelId)
        {
            await client
                .Child("userProgress")
                .Child(userId)
                .Child("completedLevels")
                .Child(levelId)
                .PutAsync(true);
        }

        public async Task<string> GetCurrentLevelAsync(string userId)
        {
            return await client
                .Child("userProgress")
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

        public async Task<Exercise> GetCurrentExerciseAsync(string userId)
        {
            return await client
                .Child("userProgress")
                .Child(userId)
                .Child("currentLevel")
                .Child("currentExercise")
                .OnceSingleAsync<Exercise>();
        }

        public Task SetCurrentExerciseAsync(string userId, Exercise exercise)
        {
            throw new System.NotImplementedException();
        }

        public Task<Dictionary<string, GeneratorState>> GetGeneratorStatesAsync(string userId)
        {
            throw new System.NotImplementedException();
        }

        public Task SetGeneratorStateAsync(string userId, string generatorId, GeneratorState state)
        {
            throw new System.NotImplementedException();
        }

        public Task<GeneratorHistory> GetGeneratorHistoryAsync(string generatorId)
        {
            throw new System.NotImplementedException();
        }

        public Task RaiseShowsCountAsync(string generatorsId)
        {
            throw new System.NotImplementedException();
        }

        public Task RaiseCorrectCountAsync(string generatorsId)
        {
            throw new System.NotImplementedException();
        }
    }
}
