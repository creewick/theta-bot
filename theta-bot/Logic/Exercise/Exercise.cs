using System;
using System.Collections.Generic;
using theta_bot.Classes;
using theta_bot.Classes.Enums;

namespace theta_bot.Logic
{
    public abstract class Exercise
    {
        public abstract IEnumerable<Complexity> GenerateOptions(Random random, int count);
        public Complexity GetComplexity() => Approximator.Estimate(this);
        public abstract string GetCode(Random random);
        public abstract int RunCode(double n);

        protected static bool Bound(double cycleVar, double prev, double n, VarType bound)
        {
            switch (bound)
            {
                case VarType.N:
                    return cycleVar < n;
                case VarType.Const:
                    return cycleVar < 2;
                case VarType.Prev:
                    return cycleVar < prev;
                default:
                    throw new NotSupportedException(bound.ToString());
            }
        }

        protected static double Step(double cycleVar, double prev, double n, OpType op, VarType step)
        {
            checked
            {
                if (op == OpType.Increase)
                    switch (step)
                    {
                        case VarType.Const:
                            return cycleVar + 1;
                        case VarType.N:
                            return cycleVar + n;
                        case VarType.Prev:
                            return cycleVar + prev;
                        default:
                            throw new NotSupportedException(step.ToString());
                    }
                switch (step)
                {
                    case VarType.Const:
                        return cycleVar * 2;
                    case VarType.N:
                        return cycleVar * n;
                    case VarType.Prev:
                        return cycleVar * Math.Max(2, prev);
                    default:
                        throw new NotSupportedException(step.ToString());
                }
            }
        }
    }
}