namespace theta_bot
{
    public class Complexity
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
        
        public static readonly Complexity[] All = 
            {Constant, Logarithmic, Linear, Polynomial, Quadratic, Cubic};
    }
}