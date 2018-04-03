using System;
using theta_bot.Generators;

namespace theta_bot
{
    public class Level1 : ILevel
    {
        public bool IsFinished(IDataProvider data, long chatId) => false;

        private readonly SimpleCodeBlock simpleCode = new SimpleCodeBlock();
        private readonly Generator[] loopGenerators =
        {
            new SimpleForLoop(),
            new LinearForLoop(),
            new LogarithmicForLoop()
        };
        
        public Task Generate(Random random)
        {
            var i = random.Next(loopGenerators.Length);
            var j = random.Next(loopGenerators.Length);
            return new Task()
                .Generate(simpleCode, random)
                .Generate(loopGenerators[i], random)
                .Generate(loopGenerators[j], random)
                .BoundVars();
        }
    }
}