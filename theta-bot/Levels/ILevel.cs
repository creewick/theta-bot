using System;
using System.Collections.Generic;

namespace theta_bot
{
    public interface ILevel
    {
        bool IsFinished(IEnumerable<bool?> stats, long chatId);
        Exercise Generate(Random random);
    }
}