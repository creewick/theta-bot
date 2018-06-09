using System;
using theta_bot.Classes.Enums;

namespace theta_bot.Logic
{
    public static class Approximator
    {
        /*
         * C n^a log^b(n) = y1
         */
        public static Complexity Estimate(Exercise exercise)
        {
            var bestDelta = double.MaxValue;
            var bestA = -1;
            var bestB = -1;
            for (var a = 0; a < 10; a++)
                for (var b = 0; b < 10; b++)
                {
                    var c = GetFactor(exercise, a, b, 100);
                    
                    var f = new Func<double, double>
                        (n => c * Math.Pow(n, a) * Math.Pow(Math.Log(n), b));
                    
                    var curDelta = Math.Abs(f(200) - exercise.RunCode(200) +
                                            f(400) - exercise.RunCode(400) +
                                            f(800) - exercise.RunCode(800));
                    if (curDelta > bestDelta) continue;
                    bestDelta = curDelta;
                    bestA = a;
                    bestB = b;
                }
            return new Complexity(bestA, bestB);
        }

        private static double GetFactor(Exercise exercise, int a, int b, int n)
            => exercise.RunCode(n) / (Math.Pow(n, a) * Math.Pow(Math.Log(n), b));
    }
}