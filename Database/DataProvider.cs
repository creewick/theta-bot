using System;
using System.Collections.Generic;
using System.Data.SQLite;
using NUnit.Framework;

namespace theta_bot
{
    public partial class DataProvider : IDataProvider
    {
        private readonly SQLiteConnection Connection;
        
        public DataProvider(string filename)
        {
            Connection = GetConnection(filename);
            Connection.Open();
            InitializeCommands();
            CreateTasksTable.ExecuteNonQuery();
            CreateStatisticsTable.ExecuteNonQuery();
            CreateTagsTable.ExecuteNonQuery();
        }
        
        private static SQLiteConnection GetConnection(string filename) => 
            new SQLiteConnection($"Data Source={filename};");
        
        public int AddTask(long chatId, string answer)
        {
            AddTaskGetId.Parameters.AddWithValue("@chat_id", chatId);
            AddTaskGetId.Parameters.AddWithValue("@answer", answer);
            using (var reader = AddTaskGetId.ExecuteReader())
                return reader.Read() ? reader.GetInt32(0) : -1;
        }

        public string GetAnswer(int taskId)
        {
            GetTask.Parameters.AddWithValue("@task_id", taskId);
            using (var reader = GetTask.ExecuteReader())
                return reader.Read() ? reader.GetString(2) : null;
        }

        public void SetSolved(int taskId, bool solved)
        {
            using (var command = SetStatistic(taskId, solved))
                command.ExecuteNonQuery();
        }

//        public bool? IsSolved(int taskId)
//        {
//            using (var command = GetStatistic(taskId))
//                using (var reader = command.ExecuteReader())
//                    return reader.Read() ? reader.GetBoolean(2) : (bool?) null;
//        }

        public IEnumerable<bool> UserStatistics(int chatId)
        {
            using (var command = GetUserStatistic(chatId))
                using (var reader = command.ExecuteReader())
                    while (reader.Read())
                        yield return reader.GetBoolean(2);
        }
    }

    [TestFixture]
    public class Tests
    {
        [Test]
        public void AddTaskNoException()
        {
            var data = new DataProvider("D:\\database");
            Console.WriteLine(data.AddTask(0, "123"));
        }

        [Test]
        public void GetAnswerSameString()
        {
            var data = new DataProvider("D:\\database");
            var id = data.AddTask(0, "123");
            Assert.AreEqual("123", data.GetAnswer(id));
        }
    }
}