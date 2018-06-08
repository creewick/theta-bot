using System;
using System.Collections.Generic;
using NUnit.Framework;
using theta_bot.Classes;
using theta_bot.Classes.Enums;

namespace theta_bot.Logic.Exercise
{
    public class SingleLoopExercise : Exercise
    {
        public readonly VarType Bound;
        public readonly OpType Operation;
        public readonly VarType Step;

        public override Complexity GetComplexity() => Approximator.Estimate(this);

        public override string GetCode(Random random, LoopType loopType)
        {
            bool positive = random.Next(2) == 1;
            var boundValue = GetRandomBound(Bound, random);
            var operation = GetOperation(Operation, positive);
            var stepValue = GetRandomStep(Step, Operation, random);
            var increase = stepValue == "1"
                ? $"i{operation}{operation}"
                : $"i{operation}={stepValue}";

            var loop = string.Format(Templates[loopType], "i=1", $"i<{boundValue}", increase);
            return $"var count=0;\n{loop}    count++;\n}}";
            
        }

        public SingleLoopExercise(VarType bound, OpType operation, VarType step)
        {
            if (bound == VarType.I) throw new ArgumentException(bound.ToString());
            if (step == VarType.I) throw new ArgumentException(step.ToString());
            Bound = bound;
            Operation = operation;
            Step = step;
        }
        
        public override IEnumerable<Complexity> GenerateOptions(Random random, int count)
        {
            throw new NotImplementedException();
        }
    }
}