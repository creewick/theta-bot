using System;
using System.Collections.Generic;
using theta_bot.Classes;
using theta_bot.Logic.Exercise;

namespace theta_bot.Levels
{
    public interface ILevel
    {
        bool IsFinished(IEnumerable<bool?> stats, long chatId);
        Exercise Generate(Random random);
    }
}