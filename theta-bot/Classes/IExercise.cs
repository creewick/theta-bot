using System;
using System.Collections.Generic;

namespace theta_bot.Classes
{
    public interface IExercise
    {
        IReadOnlyList<Tag> Tags { get; }
        Complexity Complexity { get; }
        string Code { get; }
        
        IEnumerable<Complexity> GenerateOptions(Random random, int count);
    }
}