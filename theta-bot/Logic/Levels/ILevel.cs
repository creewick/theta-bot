using System;
using System.Collections.Generic;

namespace theta_bot.Logic.Levels
{
    public interface ILevel
    {
        bool IsFinished(IEnumerable<bool?> stats, long chatId);
        Exercise.Exercise Generate(Random random);
    }
}