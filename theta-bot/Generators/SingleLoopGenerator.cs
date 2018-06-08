using System;
using theta_bot.Classes;

namespace theta_bot.Generators
{
    public class SingleLoopGenerator : Generator
    {
        IExercise Generate(Random random, VarType bound, OperationType operation, 
            VarType step, params Tag[] tags)
        {
            if (bound == VarType.I)
                throw new ArgumentException(bound.ToString());
            if (step == VarType.I)
                throw new ArgumentException(step.ToString());

            var template = GetRandomTemplate(random, operation, tags);
            var boundValue = GetRandomBound(random, bound);
            var stepValue = GetRandomStep(random, step);
        }
    }
}