using System;
using System.Collections.Generic;
using theta_bot.Classes.Enums;

namespace theta_bot.Classes
{
    public class SingleLoopExercise : IExercise
    {
        public readonly VarType Bound;
        public readonly OpType Operation;
        public readonly VarType Step;
        public readonly LoopType LoopType;
        
        public Complexity Complexity { get; }
        public string Code { get; }

        public SingleLoopExercise(VarType bound, OpType operation, VarType step, LoopType loopType)
        {
            if (bound == VarType.I) throw new ArgumentException(bound.ToString());
            if (step == VarType.I) throw new ArgumentException(step.ToString());
            Bound = bound;
            Operation = operation;
            Step = step;
            LoopType = loopType;
        }
        
        public IEnumerable<Complexity> GenerateOptions(Random random, int count)
        {
            throw new NotImplementedException();
        }
    }
}