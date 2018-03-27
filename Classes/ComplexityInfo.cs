using System.Collections.Generic;

namespace theta_bot
{
    public class ComplexityInfo
    {
        public static Dictionary<Complexity, string> All = new Dictionary<Complexity, string>
        {
            {Complexity.Constant, "Θ(1)"},
            {Complexity.Logarithmic, "Θ(logn)"},
            {Complexity.Linear, "Θ(n)"},
            {Complexity.Polynomial, "Θ(nlogn)"},
            {Complexity.Quadratic, "Θ(n²)"},
            {Complexity.Cubic, "Θ(n³)"}
        };
        
        public Complexity Value;

        public ComplexityInfo(Complexity value)
        {
            Value = value;
        }

        public void Change(Complexity value)
        {
            Value = value;
        }

        public override string ToString() => All[Value];
    }
}