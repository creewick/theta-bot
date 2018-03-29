using System;
using theta_bot.Generators;
using Telegram.Bot.Types;

namespace theta_bot.Levels
{
    public class Level0 : ILevel
    {
        public bool IsFinished(Contact person) => false;

        private readonly SimpleCodeGenerator simple = new SimpleCodeGenerator();
        private readonly Generator[] loopGenerators =
        {
            new SimpleLoopGenerator(),
            new LinearLoopGenerator(),
            new LogarithmicLoopGenerator()
        };
        
        public Exercise Generate(Random random)
        {
            var i = random.Next(loopGenerators.Length);
            return new Exercise()
                .Generate(simple, random)
                .Generate(loopGenerators[i], random)
                .BoundVars();
        }
    }
}