using System;
using System.Collections.Generic;

namespace theta_bot.Logic.Levels
{
    public interface ILevel
    {
        bool IsFinished(List<bool?> stats, long chatId);
        Exercise Generate(Random random);
    }
}