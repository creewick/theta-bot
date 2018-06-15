using System;
using System.Text;
using theta_bot.Extentions;

namespace theta_bot.Logic
{
    public struct Complexity
    {
        public readonly int n;
        public readonly int logN;
        public readonly int loglogn;

        public Complexity(int n = 0, int logN = 0, int loglogn = 0)
        {
            if (n < 0 || logN < 0) throw new ArgumentException("Numbers must be non-negative");
            this.n = n;
            this.logN = logN;
            this.loglogn = loglogn;
        }
        
        public static Complexity Const => new Complexity(0, 0);
        public static Complexity Log => new Complexity(0, 1);
        public static Complexity Log2 => new Complexity(0, 2);
        public static Complexity N => new Complexity(1, 0);
        public static Complexity NLog => new Complexity(1, 1);
        public static Complexity N2 => new Complexity(2, 0);

        public static Complexity[] All =>
            new[] {Const, Log, Log2, N, NLog, N2};
        
        public bool Equals(Complexity other)
        {
            return n == other.n && logN == other.logN;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Complexity && Equals((Complexity) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (n * 397) ^ logN;
            }
        }
        
        public override string ToString()
        {
            if (n == 0 && logN == 0) return "Θ(1)";
            var result = new StringBuilder("Θ(");
            if (n > 0)
                result.Append($"n{n.ToPower()}");
            if (logN > 0)
                result.Append($"log{logN.ToPower()}n");
            if (loglogn > 0)
                result.Append($"log(logN)");
            result.Append(")");
            return result.ToString();
        }
    }
}