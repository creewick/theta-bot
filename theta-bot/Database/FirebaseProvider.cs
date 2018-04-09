using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;

namespace theta_bot
{
    public class FirebaseProvider : IDataProvider
    {
        private readonly FirebaseClient Database;
        
        public FirebaseProvider(string url, string token)
        {
            Database = new FirebaseClient(
                url,
                new FirebaseOptions
                    {AuthTokenAsyncFactory = () => Task.FromResult(token)});
        }
        
        public string AddTask(long chatId, int level, Exercise exercise)
        {
            var key = Database
                .Child("history")
                .Child(chatId.ToString)
                .PostAsync(new InfoModel(level))
                .Result.Key;
            Database
                .Child("tasks")
                .Child(key)
                .PutAsync(new ExerciseModel(
                    exercise.Complexity.Value, 
                    exercise.Message));
            return key;
        }

        public string GetAnswer(string taskKey) => Database
            .Child("tasks")
            .Child(taskKey)
            .OnceAsync<ExerciseModel>()
            .Result.First().Object.Answer;

        public void SetSolved(long chatId, string taskKey, bool solved)
        {
            var info = Database
                .Child("history")
                .Child(chatId.ToString)
                .Child(taskKey)
                .OnceAsync<ExerciseModel>();
            
            throw new System.NotImplementedException();
        }

        public IEnumerable<bool> GetLastStats(long chatId)
        {
            throw new System.NotImplementedException();
        }

        public void SetLevel(long chatId, int level)
        {
            throw new System.NotImplementedException();
        }

        public int GetLevel(long chatId)
        {
            throw new System.NotImplementedException();
        }
    }
}