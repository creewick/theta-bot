using System;
using theta_bot.Classes.Enums;
using theta_bot.Logic.Exercise;

namespace theta_bot.Logic
{
    public static class Approximator
    {
        /*
         * C n^a log^b(n) = y1
         */
        public static Complexity Estimate(SingleLoopExercise exercise)
        {
            var delta = double.MaxValue;
            var bestA = -1;
            var bestB = -1;
            for (var a = 0; a < 10; a++)
            for (var b = 0; b < 10; b++)
            { 
                var c = GetValue(exercise, 100) / (Math.Pow(Math.Log(100), b) * Math.Pow(100, a));
                var f = new Func<double, double>(n => c * Math.Pow(n, a) * Math.Pow(Math.Log(n), b));
                var curDelta = Math.Abs(f(200) - GetValue(exercise, 200) +
                                        f(400) - GetValue(exercise, 400));
                if (!(curDelta < delta)) continue;
                delta = curDelta;
                bestA = a;
                bestB = b;
            }
            return new Complexity(bestA, bestB);
        }
        

        private static double GetValue(SingleLoopExercise exercise, int n)
        {
            var count = 0;
            for (var i = 1; Bound(i, n, exercise.Bound); i = Step(i, n, exercise.Operation, exercise.Step))
                count++;
            return count;
        }

        private static bool Bound(int i, int n, VarType bound)
        {
            switch (bound)
            {
                case VarType.N:
                    return i < n;
                case VarType.Const:
                    return i < 100;
                default:
                    throw new ArgumentOutOfRangeException(nameof(bound), bound, null);
            }
        }

        private static int Step(int i, int n, OpType op, VarType step)
        {
            return op == OpType.Increase
                ? step == VarType.Const
                    ? i + 1
                    : i + n
                : step == VarType.Const
                    ? i * 2
                    : i * n;
        }
    }
}