using System;
using System.Collections.Generic;
using theta_bot.Generators;

namespace theta_bot.Classes
{
    public class SingleLoopExercise : IExercise
    {
        public readonly VarType Bound;
        public readonly OpType Op;
        public readonly VarType Step;
        public readonly LoopType Loop;

        public Complexity Complexity { get; }
        public string Code => SingleLoopGenerator.Generate(this);

        public SingleLoopExercise(VarType bound, OpType op, 
            VarType step, LoopType loop)
        {
            if (bound == VarType.I)
                throw new ArgumentException(bound.ToString());
            if (step == VarType.I)
                throw new ArgumentException(step.ToString());
            
            Bound = bound;
            Op = op;
            Step = step;
            Loop = loop;
            
            
        }


        public IEnumerable<Complexity> GenerateOptions(Random random, int count)
        {
            throw new NotImplementedException();
        }
    }
}