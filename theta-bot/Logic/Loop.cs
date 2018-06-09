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

        public Loop(VarType bound, OpType operation, VarType step)
        {
            Bound = bound;
            Operation = operation;
            Step = step;
        }
    }
}