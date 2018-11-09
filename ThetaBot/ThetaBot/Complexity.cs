using System;
using System.Text;
using ThetaBot.Extensions;

namespace ThetaBot
{
    public class Complexity
    {
        public readonly int n;
        public readonly int logN;
        public readonly int loglogN;

        public Complexity(int n = 0, int logN = 0, int loglogN = 0)
        {
            if (n < 0 || logN < 0) throw new ArgumentException("Numbers must be non-negative");
            this.n = n;
            this.logN = logN;
            this.loglogN = loglogN;
        }

        public static Complexity Const => new Complexity(0, 0);
        public static Complexity Log => new Complexity(0, 1);
        public static Complexity Log2 => new Complexity(0, 2);
        public static Complexity N => new Complexity(1, 0);
        public static Complexity NLog => new Complexity(1, 1);
        public static Complexity N2 => new Complexity(2, 0);

        public static Complexity[] All =>
            new[] { Const, Log, Log2, N, NLog, N2 };

        public override bool Equals(object o)
        {
            return o is Complexity complexity && Equals(complexity);
        }

        public bool Equals(Complexity other)
        {
            return n == other.n && logN == other.logN && loglogN == other.loglogN;
        }

        public override string ToString()
        {
            if (n == 0 && logN == 0 && loglogN == 0) return "Θ(1)";

            return $"Θ({"n".InPower(n)}{"log".InPower(logN, "n")}{"log".InPower(loglogN, "(logN)")})";
        }
    }
}
