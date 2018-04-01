using System.Data.SQLite;

namespace theta_bot
{
    public partial class DataProvider
    {
        private SQLiteCommand CreateTasksTable;
        private SQLiteCommand CreateStatisticsTable;
        private SQLiteCommand CreateTagsTable;
        private SQLiteCommand AddTaskGetId;
        private SQLiteCommand GetTask;
        private SQLiteCommand GetStatistics;
        
        private void InitializeCommands()
        {
            CreateTasksTable = new SQLiteCommand(
                "CREATE TABLE IF NOT EXISTS tasks(" +
                "id INTEGER PRIMARY KEY AUTOINCREMENT," +
                "chat_id INTEGER," +
                "answer VARCHAR(10))",
                Connection);
            CreateStatisticsTable = new SQLiteCommand(
                "CREATE TABLE IF NOT EXISTS statistics(" +
                "id INTEGER PRIMARY KEY AUTOINCREMENT," +
                "task_id INTEGER," +
                "solved BOOLEAN)",
                Connection);
            CreateTagsTable = new SQLiteCommand(
                "CREATE TABLE IF NOT EXISTS tags(" +
                "id INTEGER PRIMARY KEY AUTOINCREMENT," +
                "task_id INTEGER," +
                "flag VARCHAR(10) )",
                Connection);
            AddTaskGetId = new SQLiteCommand(
                "INSERT INTO tasks (chat_id, answer) " +
                "VALUES (@chat_id, @answer);" +
                "SELECT LAST_INSERT_ROWID();",
                Connection);
            GetTask = new SQLiteCommand(
                "SELECT * FROM tasks WHERE id=@task_id",
                Connection);
            GetStatistics = new SQLiteCommand(
                "SELECT * FROM statistics WHERE task_id=@task_id",
                Connection);
        }

        private SQLiteCommand SetStatistic(int taskId, bool solved)
        {
            var command = new SQLiteCommand(
                "INSERT INTO statistics (task_id, solved) " +
                "VALUES (@taskId, @solved);");
            command.Parameters.Add(new SQLiteParameter("@taskId", taskId));
            command.Parameters.Add(new SQLiteParameter("@solved", solved));
            return command;
        }
        
        private SQLiteCommand GetUserStatistic(int chatId)
        {
            var command = new SQLiteCommand(
                "SELECT * FROM statistics, tasks" +
                "WHERE statistics.task_id = tasks.id" +
                "AND tasks.user_id = @user_id",
                Connection);
            command.Parameters.Add(new SQLiteParameter("@user_id", chatId));
            return command;
        }
    }
}