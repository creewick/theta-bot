using System;
using theta_bot.Classes.Enums;

namespace theta_bot.Logic
{
    public static class Approximator
    {
        // C * n^a * log^b(n) = y1
        public static Complexity Estimate(Exercise exercise)
        {
            var bestDelta = double.MaxValue;
            var bestA = -1;
            var bestB = -1;
            for (var a = 0; a < 10; a++)
                for (var b = 0; b < 10; b++)
                {
                    var c = GetFactor(exercise, a, b, 100);

                    var b1 = b;
                    var a1 = a;
                    var f = new Func<double, double>
                        (n => c * Math.Pow(n, a1) * Math.Pow(Math.Log(n), b1));
                    var delta = new Func<int, double>
                        (n => Math.Abs(
                              Math.Pow(f(n), 2) - Math.Pow(exercise.RunCode(n), 2)));
                    
                    var curDelta = delta(100) + delta(200);
                    if (curDelta >= bestDelta) continue;
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