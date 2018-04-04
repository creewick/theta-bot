using System;
using System.Collections.Generic;
using FirebaseSharp.Portable;

namespace theta_bot
{
    public class FirebaseDataProvider : IDataProvider
    {
        private readonly FirebaseApp app; 
        
        public FirebaseDataProvider(string token)
        {
            app = new FirebaseApp(
                new Uri("https://thetabot-4a6fa.firebaseio.com/"), 
                token);
            if (app.Child("tasks") == null)
                app.Child("tasks").Push();
            if (app.Child("history") == null)
                app.Child("history").Push();
        }
        
        public int AddTask(long chatId, Task task)
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