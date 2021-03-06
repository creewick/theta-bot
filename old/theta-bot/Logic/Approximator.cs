﻿﻿using System;
 using System.Collections.Generic;
 using theta_bot.Classes.Enums;

namespace theta_bot.Logic
{
    public static class Approximator
    {
        /* C * n^a * log^b(n) * log^z(log(n)) = y
         * для всех a и b делаем вот что:
         * для нескольких точек p считаем результат RunCode.
         * находим аналитически c, минимизируя средне-квадратичную ошибку для данных a и b
         * находим оптимальные a, b, c.
         */
        public static Complexity Estimate(Exercise exercise)
        {
            var bestDelta = double.MaxValue;
            var bestA = -1;
            var bestB = -1;
            var bestZ = -1;

            var values = GetValues(exercise, n => n*2, 10, 1000, 50);
            foreach (var value in values)
            {
                Console.WriteLine(value.Key + "\t" + value.Value);
            }
            for (var a = 0; a <= 3; a++)
                for (var b = 0; b <= 3; b++)
                    for (var z = 0; z <= 1; z++)
                    {
                        var c = GetFactor(values, a, b, z);
                        if (double.IsNaN(c) || double.IsInfinity(c)) continue;
                        var delta = GetDelta(values, a, b, z, c);
                        //Console.WriteLine($"{delta} : {a} {b} {z} // {c}");
                        if (delta >= bestDelta) continue;
                        bestDelta = delta;
                        bestA = a;
                        bestB = b;
                        bestZ = z;
                    }
            return new Complexity(bestA, bestB, bestZ);
        }

        private static double GetDelta(Dictionary<double, double> values, int a, int b, int z, double c)
        {
            var sum = 0.0;
            foreach (var keyPair in values)
            {
                var n = keyPair.Key;
                var d = keyPair.Value;
                var e = Math.Pow(n, a) *
                        Math.Pow(Math.Log(n), b) *
                        Math.Pow(Math.Log(Math.Log(n)), z);
                sum += Math.Pow(d - c * e, 2);
            }
            return sum / values.Count;
        }
        
        // Поиск подходящего коэффициента c :
        // f(c) = [sum (Di - c * Ni^a * log^b(Ni) * log^z(logNi))^2] / m; i = 1 to m
            
        // Положим Ei = Ni^a * log^b(Ni) * log^z(logNi)
        // f(c) = [sum (Di - c * Ei)^2] / m; i = 1 to m
        // f(c) - среднеквадратическая ошибка
            
        // df/dc = 2 * sum (Di * (1 - Ei) + c * (Ei^2 - Ei)) / m; i = 1 to m
        // Найдем экстремум, положив df/dc = 0
        // c = sum (Di * (Ei - 1)) / [sum (Ei^2) - sum (Ei)]; i = 1 to m
        private static double GetFactor(Dictionary<double, double> values, int a, int b, int z)
        {
            var numerator = 0.0;
            var denominator = 0.0;
                
            foreach (var keyPair in values)
            {
                var n = keyPair.Key;
                var d = keyPair.Value;
                var e = Math.Pow(n, a) *
                        Math.Pow(Math.Log(n), b) *
                        Math.Pow(Math.Log(Math.Log(n)), z);

                numerator += d * (e - 1);
                denominator += e * (e - 1);
            }

            if (double.IsNaN(numerator / denominator))
                return 1;
            return numerator / denominator;
        }

        private static Dictionary<double, double> GetValues(
            Exercise exercise, Func<double, double> nextN, double startValue, double stopValue, int maxCount)
        {
            var values = new Dictionary<double, double>();
            var n = startValue;
            var count = exercise.RunCode(n);
            values[n] = count;
            while (count < stopValue && values.Count < maxCount)
            {
                n = nextN(n);
                count = exercise.RunCode(n);
                values[n] = count;
            }

            return values;
        }
    }
}