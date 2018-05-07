using System;
using System.Collections.Generic;
using theta_bot.Classes;

namespace theta_bot
{
    public interface ILevel
    {
        bool IsFinished(IEnumerable<bool?> stats, long chatId);
        Exercise Generate(Random random);
    }
}