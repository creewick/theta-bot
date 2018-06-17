using System;
using theta_bot.Classes;

namespace theta_bot.Generators
{
    public class SingleLoopGenerator : Generator
    {
        public static string Generate(Random random, SingleLoopExercise exercise)
        {
            var template = GetRandomTemplate(random, operation, loopTypes);
            var boundValue = GetRandomBound(random, bound);
            var stepValue = GetRandomStep(random, step);
        }
    }
}