using System;
using System.Collections;
using theta_bot.Generators;
using Telegram.Bot.Types;

namespace theta_bot.Levels
{
    public class Level1 : ILevel
    {
        public bool IsFinished(Contact person) => false;
        private readonly SimpleCodeGenerator simple = new SimpleCodeGenerator();
        private readonly SimpleLoopGenerator loop = new SimpleLoopGenerator();
        private readonly GeneralLoopGenerator generalLoop = new GeneralLoopGenerator();
        
        public Exercise Generate(Random random)
        {
            var exercise = new Exercise()
                .Generate(simple, random);

            for (var i = 0; i < 2; i++)
            {
                if (random.Next(3) == 0)
                    exercise = exercise.Generate(loop, random);
                if (random.Next(3) == 0)
                    exercise = exercise.Generate(generalLoop, random);
            }
            
            return exercise.BoundVars();
        }
    }
}