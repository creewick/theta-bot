using System;
using System.Collections.Generic;
using System.Linq;

namespace theta_bot
{
    public class Level1 : ILevel
    {
        public bool IsFinished(IEnumerable<bool?> stats, long chatId) => 
            stats
                .Count(stat => stat != null) >= 5 &&
            stats
                .Where(stat => stat != null)
                .Take(5)
                .All(stat => (bool)stat);

        private readonly SimpleCodeBlock simpleCode = new SimpleCodeBlock();
        private readonly IGenerator[] loopGenerators =
        {
            new SimpleWhileLoop(), 
            new LinearWhileLoop(), 
        };
        
        public Exercise Generate(Random random)
        {
            var i = random.Next(loopGenerators.Length);
            return new Exercise()
                .Generate(simpleCode, random)
                .Generate(loopGenerators[i], random)
                .BoundVars();
        }
    }
}