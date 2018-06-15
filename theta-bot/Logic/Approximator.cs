using System;
using theta_bot.Classes.Enums;

namespace theta_bot.Logic
{
    public static class Approximator
    {
        // C * n^a * log^b(n) = y1
        public static Complexity Estimate(Exercise exercise)
        {
            Console.WriteLine(exercise.GetCode(new Random(1213)));
            
            var bestDelta = double.MaxValue;
            var bestA = -1;
            var bestB = -1;
            var bestZ = -1;
            var p = 20000;
            /*
             * для всех a и b делаем вот что:
             * для нескольких точек p считаем результат RunCode.
             * находим аналитически c, минимизируя средне-квадратичную ошибку для данных a и b
             * находим оптимальные a, b, c.
             */

            for (var a = 0; a < 6; a++)
            for (var b = 0; b < 6; b++)
                for (var z = 0; z <= 1; z++)
                {
                    var c = GetFactor(exercise, a, b, z, p);
                    var b1 = b;
                    var a1 = a;
                    var f = new Func<double, double>
                        (n => c * Math.Pow(n, a1) * Math.Pow(Math.Log(n), b1) * Math.Pow(Math.Log(Math.Log(n)), z));
                    var delta = new Func<int, double>
                        (n => Math.Pow(f(n) - exercise.RunCode(n), 2));
                    
                    var curDelta = delta(p/2) + delta(p*2) + delta(p*3/2) + delta(p*2/3);
                    Console.WriteLine($"D={curDelta}, abc: {a} {b} {z} → {c}");
                    if (curDelta >= bestDelta) continue;
                    bestDelta = curDelta;
                    bestA = a;
                    bestB = b;
                    bestZ = z;
                }
            return new Complexity(bestA, bestB, bestZ);
        }
            
        private static double GetFactor(Exercise exercise, int a, int b, int z, int n)
            => exercise.RunCode(n) / (Math.Pow(n, a) * Math.Pow(Math.Log(n), b) * Math.Pow(Math.Log(Math.Log(n)), z));
    }
}