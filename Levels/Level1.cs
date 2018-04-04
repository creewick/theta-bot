using System;
using System.Linq;

namespace theta_bot
{
    public class Level1 : ILevel
    {
        public bool IsFinished(IDataProvider data, long chatId)
        {
            var stats = data.GetLastStats(chatId);
            return stats.Count() >= 5 &&
                   stats
                       .Take(5)
                       .All(stat => stat);
        }

        private readonly SimpleCodeBlock simpleCode = new SimpleCodeBlock();
        private readonly Generator[] loopGenerators =
        {
            new SimpleWhileLoop(), 
            new LinearWhileLoop(), 
        };
        
        public Task Generate(Random random)
        {
            var i = random.Next(loopGenerators.Length);
            return new Task()
                .Generate(simpleCode, random)
                .Generate(loopGenerators[i], random)
                .BoundVars();
        }
    }
}