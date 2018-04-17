﻿using System;
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
            Database = new FirebaseClient(url,
                new FirebaseOptions
                    {AuthTokenAsyncFactory = () => Task.FromResult(token)});

        public string AddTask(long chatId, int level, Exercise exercise)
        {
            var key = FirebaseKeyGenerator.Next();
            Database
                .Child("history")
                .Child(chatId.ToString)
                .Child(key)
                .PutAsync(new InfoModel(level));
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
            var time = DateTime.Now;
            var data = new InfoUpdateModel 
            {
                AnswerTime = time, 
                State = solved,
                Timestamp = (int)time.TimeOfDay.TotalMilliseconds
            };
            Database
                .Child("history")
                .Child(chatId.ToString)
                .Child(taskKey)
                .PatchAsync(data);
        }

        public IEnumerable<bool?> GetLastStats(long chatId, int count) => 
            Database
                .Child("history")
                .Child(chatId.ToString)
                .OrderBy("Timestamp")
                .LimitToLast(count)
                .OnceAsync<InfoModel>()
                .Result
                .Select(v => v.Object.State);

        public void SetLevel(long chatId, int level) => 
            Database
                .Child("userStats")
                .Child(chatId.ToString)
                .Child("level")
                .PutAsync(level);

        public int? GetLevel(long chatId) => 
            Database
                .Child("userStats")
                .Child(chatId.ToString)
                .Child("level")
                .OnceSingleAsync<int>()
                .Result;
    }
}