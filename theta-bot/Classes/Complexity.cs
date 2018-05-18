using System;
using theta_bot.Extentions;

namespace theta_bot.Classes
{
    public class Complexity
    {
        public readonly int N;
        public readonly int LogN;

        public Complexity(int n, int logN)
        {
            if (n < 0 || logN < 0) throw new ArgumentException("Numbers must be non-negative");
            N = n;
            LogN = logN;
        }

        public override string ToString()
        {
            if (N == 0)
                return LogN == 0
                    ? "Θ(1)"
                    : $"Θ(log{LogN.ToPower()}n)";
            return LogN == 0
                ? $"Θ(n{N.ToPower()})"
                : $"Θ(n{N.ToPower()}log{LogN.ToPower()}n)";
        }
    }
}