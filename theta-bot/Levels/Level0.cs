using System;
using System.Linq;

namespace theta_bot
{
    public class Level0 : ILevel
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
        private readonly IGenerator[] loopGenerators =
        {
            new SimpleForLoop(),
            new LinearForLoop(),
            new LogarithmicForLoop()
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