using System;

namespace theta_bot
{
    public struct Complexity
    {
        public readonly string Value;
        
        
        private Complexity(string value) => Value = value;

        public static Complexity[] All => 
            new[] {Constant, Logarithmic, Linear, Polynomial, Quadratic, Cubic};
        public static Complexity Constant => 
            new Complexity("Θ(1)");
        public static Complexity Logarithmic =>
            new Complexity("Θ(logn)");
        public static Complexity Linear =>
            new Complexity("Θ(n)");
        public static Complexity Polynomial =>
            new Complexity("Θ(nlogn)");
        public static Complexity Quadratic =>
            new Complexity("Θ(n²)");
        public static Complexity Cubic =>
            new Complexity("Θ(n³)");
    }
}