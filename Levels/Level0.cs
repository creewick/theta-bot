using System;
using theta_bot.Generators;
using Telegram.Bot.Types;

namespace theta_bot
{
    public class Level0 : ILevel
    {
        public bool IsFinished(IDataProvider data, long chatId) => false;

        private readonly SimpleCodeGenerator simple = new SimpleCodeGenerator();
        private readonly Generator[] loopGenerators =
        {
            new SimpleLoopGenerator(),
            new LinearLoopGenerator(),
            new LogarithmicLoopGenerator()
        };
        
        public Task Generate(Random random)
        {
            var i = random.Next(loopGenerators.Length);
            return new Task()
                .Generate(simple, random)
                .Generate(loopGenerators[i], random)
                .BoundVars();
        }
    }
}