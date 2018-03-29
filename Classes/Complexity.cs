using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace theta_bot
{
    public struct Complexity
    {
        public readonly string Value;

        private Complexity(string value)
        {
            Value = value;
        }

        public static Complexity Constant { get; } = new Complexity("Θ(1)");
        public static Complexity Logarithmic { get; } = new Complexity("Θ(logn)");
        public static Complexity Linear { get; } = new Complexity("Θ(n)");
        public static Complexity Polynomial { get; } = new Complexity("Θ(nlogn)");
        public static Complexity Quadratic { get; } = new Complexity("Θ(n²)");
        public static Complexity Cubic { get; } = new Complexity("Θ(n³)");

        public static IEnumerable<Complexity> All { get; } =
            typeof(Complexity)
                .GetProperties(BindingFlags.Static | BindingFlags.Public)
                .Where(p => p.PropertyType == typeof(Complexity))
                .Select(p => (Complexity) p.GetValue(null));
    }
}