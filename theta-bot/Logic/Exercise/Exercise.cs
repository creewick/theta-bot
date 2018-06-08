using System;
using System.Collections.Generic;
using theta_bot.Classes;
using theta_bot.Classes.Enums;

namespace theta_bot.Logic.Exercise
{
    public abstract class Exercise
    {
        public abstract Complexity GetComplexity();
        public abstract string GetCode(Random random, LoopType loopType);

        protected static readonly Dictionary<LoopType, string> Templates =
            new Dictionary<LoopType, string>
        {
            [LoopType.For] = "for (var {0}; {1}; {2})\n{{\n",
            [LoopType.While] = "var {0};\nwhile ({1})\n{{\n    {2};\n"
        };

        protected static string GetRandomBound(VarType bound, Random random)
        {
            switch (bound)
            {
                case VarType.Const:
                    return (random.Next(1, 5) * 100).ToString();
                case VarType.N:
                    return "n";
                case VarType.I:
                    return "i";
                default:
                    throw new ArgumentOutOfRangeException(nameof(bound), bound, null);
            }
        }

        protected static string GetOperation(OpType operation, bool positive)
        {
            return positive
                ? operation == OpType.Increase
                    ? "+"
                    : "*"
                : operation == OpType.Increase
                    ? "-"
                    : ":";
        }

        protected static string GetRandomStep(VarType step, OpType operation, Random random)
        {
            switch (step)
            {
                case VarType.Const:
                    if (operation == OpType.Multiply)
                        return random.Next(2, 5).ToString();
                    return random.Next(1, 5).ToString();
                case VarType.N:
                    return "n";
                case VarType.I:
                    return "i";
                default:
                    throw new ArgumentOutOfRangeException(nameof(step), step, null);
            }
        }
        
        public abstract IEnumerable<Complexity> GenerateOptions(Random random, int count);
    }
}