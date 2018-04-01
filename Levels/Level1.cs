using System;
using theta_bot.Generators;

namespace theta_bot
{
    public class Level1 : ILevel
    {
        public bool IsFinished(IDataProvider data, long chatId) => false;

        private readonly SimpleCodeGenerator simpleCode = new SimpleCodeGenerator();
        private readonly Generator[] loopGenerators =
        {
            new SimpleLoopGenerator(),
            new LinearLoopGenerator(),
            new LogarithmicLoopGenerator()
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