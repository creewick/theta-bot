using System;
using System.Collections.Generic;
using theta_bot.Classes;
using theta_bot.Classes.Enums;

namespace theta_bot.Logic
{
    public class CodeGenerator
    {
        private static readonly Dictionary<LoopType, string> Templates =
            new Dictionary<LoopType, string>
        {
            [LoopType.For] = "for (var {0}; {1}; {2})\n{{\n",
            [LoopType.While] = "var {0};\nwhile ({1})\n{{\n    {2};\n"
        };

        public static string GetLoopCode(Loop loop, LoopType loopType, Random random)
        {
            bool positive = random.Next(2) == 1;
            
            var boundValue = GetRandomBound(loop.Bound, random);
            var operation = GetOperation(loop.Operation, positive);
            var stepValue = GetRandomStep(loop.Step, loop.Operation, random);

            var assign = positive
                ? "i=1"
                : $"i={boundValue}";
            var condition = positive
                ? $"i<{boundValue}"
                : $"i>1";
            var increase = stepValue == "1"
                ? $"i{operation}{operation}"
                : $"i{operation}={stepValue}";

            return string.Format(Templates[loopType], assign, condition, increase);
        }

        private static string GetRandomBound(VarType bound, Random random)
        {
            switch (bound)
            {
                case VarType.Const:
                    return (random.Next(1, 5) * 100).ToString();
                case VarType.N:
                    return "n";
                case VarType.Prev:
                    return "i";
                default:
                    throw new ArgumentOutOfRangeException(nameof(bound), bound, null);
            }
        }

        private static string GetOperation(OpType operation, bool positive)
        {
            return positive
                ? operation == OpType.Increase
                    ? "+"
                    : "*"
                : operation == OpType.Increase
                    ? "-"
                    : "/";
        }

        private static string GetRandomStep(VarType step, OpType operation, Random random)
        {
            switch (step)
            {
                case VarType.Const:
                    if (operation == OpType.Multiply)
                        return random.Next(2, 5).ToString();
                    return random.Next(1, 5).ToString();
                case VarType.N:
                    return "n";
                case VarType.Prev:
                    return "i";
                default:
                    throw new ArgumentOutOfRangeException(nameof(step), step, null);
            }
        }
    }
}