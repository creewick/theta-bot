using System;
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
        
        public FirebaseProvider(string url, string token) => 
            Database = new FirebaseClient(
                url,
                new FirebaseOptions
                    {AuthTokenAsyncFactory = () => Task.FromResult(token)});

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

        public string GetAnswer(string taskKey) => 
            Database
            .Child("tasks")
            .Child(taskKey)
            .Child("Answer")
            .OnceSingleAsync<string>()
            .Result;
        
        public void SetSolved(long chatId, string taskKey, bool solved)
        {
            var info = Database
                .Child("history")
                .Child(chatId.ToString)
                .Child(taskKey)
                .OnceSingleAsync<InfoModel>()
                .Result;
            
            info.AnswerTime = DateTime.Now;
            info.State = solved;
            
            Database
                .Child("history")
                .Child(chatId.ToString)
                .Child(taskKey)
                .PutAsync(info);
        }

        public IEnumerable<bool> GetLastStats(long chatId) => new List<bool>();
//            Database
//                .Child("history")
//                .Child(chatId.ToString)
//                .OrderBy("AnswerTime")
//                .OnceSingleAsync<Dictionary<string, InfoModel>>()
//                .Result
//                .Select(pair => pair.Value)
//                .Where(res => res.State != null)
//                .Select(res => (bool)res.State);

        public void SetLevel(long chatId, int level) => 
            Database
            .Child("userStats")
            .Child(chatId.ToString)
            .Child("level")
            .PutAsync(level);

        public int? GetLevel(long chatId) => Database
            .Child("userStats")
            .Child(chatId.ToString)
            .Child("level")
            .OnceAsync<int>()
            .Result.FirstOrDefault()?.Object;
    }
}