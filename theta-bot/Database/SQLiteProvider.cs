using System.Collections.Generic;
using System.Data.SQLite;

namespace theta_bot
{
    public class SQLiteProvider : IDataProvider
    {
        private readonly SQLiteConnection Connection;
        
        public SQLiteProvider(string filename)
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
                "chat_id INTEGER," +
                "level INTEGER)",
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

        public IEnumerable<bool> GetLastStats(long chatId)
        {
            var result = new List<bool>();
            var command = new SQLiteCommand(
                "SELECT s.solved FROM tasks AS t " +
                "LEFT JOIN statistics AS s " +
                "ON t.id = s.task_id " +
                "WHERE t.chat_id=@chat_id AND s.solved IS NOT NULL " +
                "ORDER BY s.id DESC LIMIT 10", 
                Connection);
            command.Parameters.AddWithValue("@chat_id", chatId);
            using (var reader = command.ExecuteReader())
                while (reader.Read())
                    result.Add(reader.GetBoolean(0));
            return result;
        }

        public void SetLevel(long chatId, int level)
        {
            var command = new SQLiteCommand(
                "INSERT INTO levels (chat_id, level) VALUES (@chat_id, @level)",
                Connection);
            command.Parameters.AddWithValue("@chat_id", chatId);
            command.Parameters.AddWithValue("@level", level);
            command.ExecuteNonQuery();
        }

        public int GetLevel(long chatId)
        {
            var command = new SQLiteCommand(
                "SELECT level FROM levels " +
                "WHERE chat_id = @chat_id " +
                "ORDER BY id DESC LIMIT 1",
                Connection);
            command.Parameters.AddWithValue("@chat_id", chatId);
            using (var reader = command.ExecuteReader())
                return reader.Read() ? reader.GetInt32(0) : -1;
        }
    }
}