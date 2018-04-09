using System;
using System.Collections.Generic;
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
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(token)
                }
            );
            var a = Database.Child("zakhar").PostAsync(2);
            Console.WriteLine(a.Result.Object);
        }
        
        public int AddTask(long chatId, string answer)
        {
            throw new System.NotImplementedException();
        }

        public string GetAnswer(int taskId)
        {
            throw new System.NotImplementedException();
        }

        public void SetSolved(int taskId, bool solved)
        {
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