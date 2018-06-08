using System;
using System.Collections.Generic;
using theta_bot.Classes;

namespace theta_bot.Levels
{
    public interface ILevel
    {
        bool IsFinished(IEnumerable<bool?> stats, long chatId);
        IExercise Generate(Random random);
    }
}