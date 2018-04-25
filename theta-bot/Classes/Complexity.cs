using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace theta_bot
{
    public class Complexity
    {
        public readonly string Value;
        private readonly int Priority;

        private Complexity(string value, int priority)
        {
            Value = value;
            Priority = priority;
        }

        public static readonly Complexity Constant = new Complexity("Θ(1)", 0);
        public static readonly Complexity LogN = new Complexity("Θ(logn)", 1);
        public static readonly Complexity Log2N = new Complexity("Θ(log²n)", 2);
        public static readonly Complexity N = new Complexity("Θ(n)", 3);
        public static readonly Complexity NLogN = new Complexity("Θ(nlogn)", 4);
        public static readonly Complexity N2 = new Complexity("Θ(n²)", 5);
        public static readonly Complexity N3 = new Complexity("Θ(n³)", 6);

        public static readonly IEnumerable<Complexity> All =
            typeof(Complexity)
                .GetFields(BindingFlags.Static | BindingFlags.Public)
                .Where(p => p.FieldType == typeof(Complexity))
                .Select(p => (Complexity) p.GetValue(null));

        public static bool operator <(Complexity c1, Complexity c2) => c1.Priority < c2.Priority;
        public static bool operator >(Complexity c1, Complexity c2) => c1.Priority > c2.Priority;
        public static bool operator <=(Complexity c1, Complexity c2) => c1.Priority <= c2.Priority;
        public static bool operator >=(Complexity c1, Complexity c2) => c1.Priority >= c2.Priority;
    }
}