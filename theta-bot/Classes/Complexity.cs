using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace theta_bot
{
    public class Complexity
    {
        public readonly string Value;

        private Complexity(string value)
        {
            Value = value;
        }

        public static readonly Complexity Constant = new Complexity("Θ(1)");
        public static readonly Complexity Logarithmic = new Complexity("Θ(logn)");
        public static readonly Complexity Linear = new Complexity("Θ(n)");
        public static readonly Complexity Polynomial = new Complexity("Θ(nlogn)");
        public static readonly Complexity Quadratic = new Complexity("Θ(n²)");
        public static readonly Complexity Cubic = new Complexity("Θ(n³)");

        public static readonly IEnumerable<Complexity> All =
            typeof(Complexity)
                .GetFields(BindingFlags.Static | BindingFlags.Public)
                .Where(p => p.FieldType == typeof(Complexity))
                .Select(p => (Complexity) p.GetValue(null));
    }
}