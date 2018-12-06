﻿using System.Threading.Tasks;
using Theta_Bot.Database;

namespace Theta_Bot.Logic
{
    public class ThetaBot
    {
        private readonly IDataProvider database;

        public ThetaBot(IDataProvider database)
        {
            this.database = database;
        }

        public async Task<Exercise> GetExercise(string userId)
        {
            if (await UserHaveTask(userId))
                return null;

            var level = await database.GetCurrentExerciseAsync(userId);

        }

        private async Task<bool> UserHaveTask(string userId)
        {
            return await database.GetCurrentExerciseAsync(userId) != null;
        }
    }
}