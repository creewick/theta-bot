using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using Newtonsoft.Json.Bson;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace theta_bot
{
    public class ThetaBot
    {
        private readonly TelegramBotClient bot;
        private readonly ILevel[] levels;
        private readonly Random random = new Random();
        private readonly SQLiteConnection databaseConnection;
        
        public ThetaBot(string token, ILevel[] levels, string filename)
        {
            bot = new TelegramBotClient(token);
            this.levels = levels;
            databaseConnection = new SQLiteConnection($"Data Source={filename};");
            PrepareDatabase();
            
            bot.OnMessage += OnMessageReceive;
            
            bot.StartReceiving();
            Console.ReadLine();
            bot.StopReceiving();
        }

        private async void OnMessageReceive(object sender, MessageEventArgs args)
        {
            var message = args.Message;
            Console.WriteLine(CheckAnswer(message.Contact.UserId, message.Text));
            
            var exercise = GetExercise(message.Contact);
            RememberAnswer(message.Contact.UserId, exercise.Complexity.Value);
            
            await bot.SendTextMessageAsync(
                message.Chat.Id, 
                exercise.GetMessage(), 
                ParseMode.Markdown, 
                false, false, 0, 
                GetKeyboard(exercise.GetOptions(random, 4)));
        }

        private bool CheckAnswer(int userId, string answer)
        {
            var command = new SQLiteCommand($"SELECT correct FROM Exercises WHERE userid = {userId}", 
                databaseConnection);
            var reader = command.ExecuteReader();
            foreach (DbDataRecord record in reader)
                return answer == (string)record["correct"];
            return false;
        }
        
        private void RememberAnswer(int userId, string answer)
        {
            new SQLiteCommand("UPDATE OR INSERT INTO Exercises (userid, corrent) VALUES " +
                             $"({userId}, {answer})", 
                databaseConnection).ExecuteNonQuery();
        }
        
        private Exercise GetExercise(Contact person)
        {
            // TODO: Узнать уровень игрока
            return levels[0].Generate(random).BoundVars();
        }
        
        private static ReplyKeyboardMarkup GetKeyboard(IEnumerable<string> labels)
        {
            var keyboard = labels
                .Select(option => new KeyboardButton(option))
                .ToArray();
            return new ReplyKeyboardMarkup(keyboard, true, true);
        }

        private void PrepareDatabase()
        {
            databaseConnection.Open();
            new SQLiteCommand("CREATE TABLE IF NOT EXISTS Exercises (" +
                              "userid INTEGER PRIMARY KEY," +
                              "correct VARCHAR(10))", databaseConnection).ExecuteNonQuery();
        }
    }
}