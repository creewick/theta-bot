using System;
using System.Collections.Generic;
using FireSharp;
using FireSharp.Config;

namespace theta_bot
{
    public class FireSharpProvider : IDataProvider
    {
        private readonly FirebaseClient Database;

        public FireSharpProvider(string url, string token) =>
            Database = new FirebaseClient(
                new FirebaseConfig {AuthSecret = token, BasePath = url});
        
        public string AddTask(long chatId, int level, Exercise exercise)
        {
            var key = Database.PushAsync(
                $"history/{chatId}", 
                new InfoModel(level))
                .Result;
            Database.Set($"tasks/{key.Result.name}", new ExerciseModel(
                exercise.Complexity.Value,
                exercise.Message));
            return key.Result.name;
        }

        public string GetAnswer(string taskKey) => 
            Database
                .GetAsync($"tasks/{taskKey}/Answer")
                .Result
                .ResultAs<string>();

        public void SetSolved(long chatId, string taskKey, bool solved)
        {
            var info = Database
                .GetAsync($"history/{chatId}/{taskKey}")
                .Result
                .ResultAs<InfoModel>();
            info.AnswerTime = DateTime.Now;
            info.State = solved;
            Database
                .Set($"history/{chatId}/{taskKey}", info);
        }

        public IEnumerable<bool> GetLastStats(long chatId) => new List<bool>();

        public void SetLevel(long chatId, int level) => 
            Database
                .Set($"userStats/{chatId}/level", level);

        public int? GetLevel(long chatId) => 
            Database
                .GetAsync($"userStats/{chatId}/level")
                .Result
                .ResultAs<int?>();
    }
}