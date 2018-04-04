using System;
using System.Collections.Generic;
using System.Text;

namespace theta_bot
{
    public class LinearWhileLoop: Generator
    {
        private readonly Dictionary<Complexity, Complexity> complexities = 
            new Dictionary<Complexity, Complexity>
            {
                {Complexity.Constant, Complexity.Linear},
                {Complexity.Logarithmic, Complexity.Polynomial},
                {Complexity.Linear, Complexity.Quadratic},
                {Complexity.Quadratic, Complexity.Cubic}
            };
        
        private readonly string[] templates =
        {
            "while ({0} < {2} / 2)\n{{\n    {0}++;\n",
            "while ({0} < {2})\n{{\n    {0}+={3};\n",
            "while ({0} < {2})\n{{\n    {0} = {0} + {3};\n",
        };
        
        public override void ChangeCode(StringBuilder code, List<Variable> vars, Random random)
        {
            var variable = GetNextVar(vars, random);
            var startValue = random.Next(2);
            var stepValue = random.Next(1, 5);
            var template = templates[random.Next(templates.Length)];
            var newCode = string.Format(template, variable.Label, startValue, "n", stepValue);
        
            code.ShiftLines(4);
            code.Insert(0, newCode);
            code.Append("}\n");
        }

        public override bool TryGetComplexity(Complexity oldComplexity, out Complexity newComplexity)
        {
            newComplexity = complexities.ContainsKey(oldComplexity)
                ? complexities[oldComplexity]
                : oldComplexity;
            return complexities.ContainsKey(oldComplexity);
        }
    }
}