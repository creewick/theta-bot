using System;
using System.Collections.Generic;
using System.Data.SQLite;
using NUnit.Framework;

namespace theta_bot
{
    public class DataProvider : IDataProvider
    {
        private readonly SQLiteConnection Connection;
        
        public DataProvider(string filename)
        {
            Connection = GetConnection(filename);
            Connection.Open();
            new SQLiteCommand(
                "CREATE TABLE IF NOT EXISTS tasks(" +
                "id INTEGER PRIMARY KEY AUTOINCREMENT," +
                "chat_id INTEGER," +
                "answer VARCHAR(10) )",
                Connection).ExecuteNonQuery();
            new SQLiteCommand(
                "CREATE TABLE IF NOT EXISTS statistics(" +
                "id INTEGER PRIMARY KEY AUTOINCREMENT," +
                "task_id INTEGER," +
                "solved BOOLEAN)",
                Connection).ExecuteNonQuery();
            new SQLiteCommand(
                "CREATE TABLE IF NOT EXISTS tags(" +
                "id INTEGER PRIMARY KEY AUTOINCREMENT," +
                "task_id INTEGER," +
                "flag VARCHAR(10) )",
                Connection).ExecuteNonQuery();
            new SQLiteCommand(
                "CREATE TABLE IF NOT EXISTS levels(" +
                "id INTEGER PRIMARY KEY AUTOINCREMENT," +
                "user_id INTEGER," +
                "level VARCHAR(10) )",
                Connection).ExecuteNonQuery();
        }
        
        private static SQLiteConnection GetConnection(string filename) => 
            new SQLiteConnection($"Data Source={filename};");
        
        public int AddTask(long chatId, string answer)
        {
            var command = new SQLiteCommand(
                "INSERT INTO tasks (chat_id, answer) " +
                "VALUES (@chat_id, @answer);" +
                "SELECT LAST_INSERT_ROWID();",
                Connection);
            command.Parameters.AddWithValue("@chat_id", chatId);
            command.Parameters.AddWithValue("@answer", answer);
            using (var reader = command.ExecuteReader())
                return reader.Read() ? reader.GetInt32(0) : -1;
        }

        public string GetAnswer(int taskId)
        {
            var command = new SQLiteCommand(
                "SELECT * FROM tasks WHERE id=@task_id",
                Connection);
            command.Parameters.AddWithValue("@task_id", taskId);
            using (var reader = command.ExecuteReader())
                return reader.Read() ? reader.GetString(2) : null;
        }

        public void SetSolved(int taskId, bool solved)
        {
            var command = new SQLiteCommand(
                "INSERT INTO statistics (task_id, solved) " +
                "VALUES (@task_id, @solved);",
                Connection);
            command.Parameters.AddWithValue("@task_id", taskId);
            command.Parameters.AddWithValue("@solved", solved);
            command.ExecuteNonQuery();
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
            var id = data.AddTask(0, "0");
            var id2 = data.AddTask(1, "1");
            Assert.AreEqual("0", data.GetAnswer(id));
            Assert.AreEqual("1", data.GetAnswer(id2));
        }
    }
}