using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;

namespace Theta_Bot.Database
{
    public class FirebaseDataProvider : IDataProvider
    {
        private readonly FirebaseClient client;

        public FirebaseDataProvider(string databaseUrl, string databaseSecret)
        {
            client = new FirebaseClient(databaseUrl,
                new FirebaseOptions
                    {AuthTokenAsyncFactory = () => Task.FromResult(databaseSecret)});
        }

        public async Task<Dictionary<string, bool>> GetCompletedLevels(string userId)
        {
            return await client
                .Child("userProgress")
                .Child(userId)
                .Child("completedLevels")
                .OnceSingleAsync<Dictionary<string, bool>>();
        }

        public async Task SetLevelAsCompleted(string userId, string levelId)
        {
            await client
                .Child("userProgress")
                .Child(userId)
                .Child("completedLevels")
                .Child(levelId)
                .PutAsync(true);
        }

        public async Task<string> GetCurrentLevel(string userId)
        {
            return await client
                .Child("userProgress")
                .Child(userId)
                .Child("currentLevel")
                .Child("levelId")
                .OnceSingleAsync<string>();
        }

        public async Task SetCurrentLevel(string userId, string levelId)
        {
            await client
                .Child("userProgress")
                .Child(userId)
                .Child("currentLevel")
                .Child("levelId")
                .PutAsync(levelId);
        }

        public async Task<Exercise> GetCurrentExercise(string userId)
        {
            return await client
                .Child("userProgress")
                .Child(userId)
                .Child("currentLevel")
                .Child("currentExercise")
                .OnceSingleAsync<Exercise>();
        }

        public Task SetCurrentExercise(string userId, Exercise exercise)
        {
            throw new System.NotImplementedException();
        }

        public Task<Dictionary<string, GeneratorState>> GetGeneratorStates(string userId)
        {
            throw new System.NotImplementedException();
        }

        public Task SetGeneratorState(string userId, string generatorId, GeneratorState state)
        {
            throw new System.NotImplementedException();
        }

        public Task<GeneratorHistory> GetGeneratorHistory(string generatorId)
        {
            throw new System.NotImplementedException();
        }

        public Task RaiseShowsCount(string generatorsId)
        {
            throw new System.NotImplementedException();
        }

        public Task RaiseCorrectCount(string generatorsId)
        {
            throw new System.NotImplementedException();
        }
    }
}
