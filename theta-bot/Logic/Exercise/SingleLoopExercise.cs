using System;
using System.Collections.Generic;
using theta_bot.Classes;

namespace theta_bot.Logic
{
    public class SingleLoopExercise : Exercise
    {
        public override Complexity GetComplexity() => Approximator.Estimate(this);

        public readonly Loop Loop;
        public readonly LoopType LoopType;

        public SingleLoopExercise(Loop loop, LoopType loopType)
        {
            if (!loop.OuterLoop) throw new ArgumentException("Single loop must be created as outer loop");
            Loop = loop;
            LoopType = loopType;
        }
        
        public override string GetCode(Random random)
        {
            bool positive = random.Next(2) == 1;
            var boundValue = GetRandomBound(Loop.Bound, random);
            var operation = GetOperation(Loop.Operation, positive);
            var stepValue = GetRandomStep(Loop.Step, Loop.Operation, random);
            var increase = stepValue == "1"
                ? $"i{operation}{operation}"
                : $"i{operation}={stepValue}";

            var loop = string.Format(Templates[LoopType], "i=1", $"i<{boundValue}", increase);
            return $"var count=0;\n{loop}    count++;\n}}";
        }


        
        public override IEnumerable<Complexity> GenerateOptions(Random random, int count)
        {
            throw new NotImplementedException();
        }
    }
}