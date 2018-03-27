using System;
using theta_bot.Generators;

namespace theta_bot
{
    public abstract class Generator
    {
        protected abstract void ChangeCode(Exercise exercise, Random random);
        protected abstract Complexity GetComplexity(Complexity previousComplexity);

        public Exercise Generate(Exercise exercise, Random random)
        {
            ChangeCode(exercise, random);
            exercise.Complexity = Complexity.Constant;
            return new Exercise();
        }
    }
}