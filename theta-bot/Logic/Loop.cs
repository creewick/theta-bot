using System;
using theta_bot.Classes;
using theta_bot.Classes.Enums;

namespace theta_bot.Logic
{
    public class Loop
    {
        public readonly OpType Operation;
        public readonly VarType Bound;
        public readonly VarType Step;

        public Loop(VarType bound, OpType operation, VarType step)
        {
            Operation = operation;
            Bound = bound;
            Step = step;
        }
    }
}