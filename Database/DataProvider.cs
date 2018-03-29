using System.Data.Common;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.SQLite;

namespace theta_bot
{
    public class DataProvider : IDataProvider
    {
        private readonly SQLiteConnection Connection;
        
        public DataProvider(string filename)
        {
            Connection = new SQLiteConnection($"Data Source={filename};");
            Connection.Open();
            new SQLiteCommand("CREATE TABLE IF NOT EXISTS Exercises(" +
                              "chatid INTEGER PRIMARY KEY, " +
                              "answer VARCHAR(10) )", Connection).ExecuteNonQuery();
        }
        
        public string GetAnswer(long chatId)
        {
            var command = new SQLiteCommand(
                "SELECT answer FROM Exercises WHERE chatid = @id", 
                Connection);
            command.Parameters.Add(new SQLiteParameter("@id", chatId));
            
            var reader = command.ExecuteReader();
            return ((DbDataRecord)reader[0])["answer"].ToString();
        }

        public void StoreAnswer(long chatId, string answer)
        {
            var command = new SQLiteCommand(
                "REPLACE INTO Exercises (chatid, answer) VALUES (@id, @answer)",
                Connection);
            command.Parameters.Add(new SQLiteParameter("@id", chatId));
            command.Parameters.Add(new SQLiteParameter("@answer", answer));
            command.ExecuteNonQuery();
        }
    }
}