using System;
using System.Collections.Generic;
using theta_bot.Classes;
using theta_bot.Classes.Enums;

namespace theta_bot.Logic
{
    public class SingleLoopExercise : Exercise
    {
        public readonly Loop Loop;
        public readonly LoopType LoopType;

        public SingleLoopExercise(Loop loop, LoopType loopType)
        {
            if (loop.Bound == VarType.Prev || loop.Step == VarType.Prev) 
                throw new ArgumentException("Outer cycle can't depend on I");
            Loop = loop;
            LoopType = loopType;
        }
        
        public override string GetCode(Random random)
        {
            var loop = CodeGenerator.GetLoopCode(Loop, LoopType, random);
            return $"var count=0;\n{loop}    count++;\n}}";
        }

        public override int RunCode(int n)
        {
            var b = Loop.Bound;
            var op = Loop.Operation;
            var s = Loop.Step;
            
            var count = 0;
            for (var i = 1; Bound(i, 0, n, b); i = Step(i, 0, n, op, s))
                count++;
            return count;
        }
        
        public override IEnumerable<Complexity> GenerateOptions(Random random, int count)
        {
            throw new NotImplementedException();
        }
    }
}