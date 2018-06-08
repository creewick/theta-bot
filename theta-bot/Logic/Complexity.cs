using System;
using System.Text;
using theta_bot.Extentions;

namespace theta_bot.Logic
{
    public struct Complexity
    {
        public readonly int N;
        public readonly int LogN;
        
        public Complexity(int n=0, int logN=0)
        {
            if (n < 0 || logN < 0) throw new ArgumentException("Numbers must be non-negative");
            N = n;
            LogN = logN;
        }
        
        public static Complexity Const => new Complexity(0, 0);
        public static Complexity Log => new Complexity(0, 1);
        public static Complexity Linear => new Complexity(1, 0);
        
        public bool Equals(Complexity other)
        {
            return N == other.N && LogN == other.LogN;
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
                return (N * 397) ^ LogN;
            }
        }
        
        public override string ToString()
        {
            if (N == 0 && LogN == 0) return "Θ(1)";
            var result = new StringBuilder("Θ(");
            if (N > 0)
                result.Append($"n{N.ToPower()}");
            if (LogN > 0)
                result.Append($"log{LogN.ToPower()}n");
            result.Append(")");
            return result.ToString();
        }
    }
}