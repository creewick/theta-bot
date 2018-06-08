using System;
using System.Collections.Generic;

namespace theta_bot.Classes
{
    public class Exercise : IExercise
    {
        public IReadOnlyList<Tag> Tags { get; }
        public Complexity Complexity { get; }
        public string Code { get; }

        public Exercise(string code, Complexity complexity, params Tag[] tags)
        {
            Code = code;
            Complexity = complexity;
            Tags = tags;
        }
        
        public IEnumerable<Complexity> GenerateOptions(Random random, int count)
        {
            throw new NotImplementedException();
        }
    }
}