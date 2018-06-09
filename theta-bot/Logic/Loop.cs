using System;
using theta_bot.Classes;
using theta_bot.Classes.Enums;

namespace theta_bot.Logic
{
    public class Loop
    {
        public readonly VarType Bound;
        public readonly OpType Operation;
        public readonly VarType Step;
        public readonly bool OuterLoop;

        public Loop(VarType bound, OpType operation, VarType step, bool outerLoop)
        {
            if (outerLoop && (bound == VarType.I || step == VarType.I))
                throw new ArgumentException("Outer loop can't depend on I");
            Bound = bound;
            Operation = operation;
            Step = step;
            OuterLoop = outerLoop;
        }
    }
}