using System;
using System.Collections;
using theta_bot.Generators;
using Telegram.Bot.Types;

namespace theta_bot.Levels
{
    public class Level1 : ILevel
    {
        public bool IsFinished(IDataProvider data, long chatId) => false;
        private readonly SimpleCodeGenerator simple = new SimpleCodeGenerator();
        private readonly SimpleLoopGenerator loop = new SimpleLoopGenerator();
        private readonly LinearLoopGenerator linearLoop = new LinearLoopGenerator();
        
        public Exercise Generate(Random random)
        {
            var exercise = new Exercise()
                .Generate(simple, random);

            for (var i = 0; i < 2; i++)
            {
                if (random.Next(3) == 0)
                    exercise = exercise.Generate(loop, random);
                if (random.Next(3) == 0)
                    exercise = exercise.Generate(simple, random);
                if (random.Next(3) == 0)
                    exercise = exercise.Generate(linearLoop, random);
            }
            
            return exercise.BoundVars();
        }
    }
}