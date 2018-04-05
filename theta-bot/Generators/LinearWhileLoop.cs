using System;
using System.Collections.Generic;
using System.Text;

namespace theta_bot
{
    public class LinearWhileLoop: IGenerator
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
            "var {0} = {1};\nwhile ({0} < {2} / 2)\n{{\n    {0}++;\n",
            "var {0} = {1};\nwhile ({0} < {2})\n{{\n    {0}+={3};\n",
            "var {0} = {1};\nwhile ({0} < {2})\n{{\n    {0} = {0} + {3};\n",
        };
        
        public void ChangeCode(StringBuilder code, Func<Variable> getNextVar, Random random)
        {
            var variable = getNextVar();
            var startValue = random.Next(2);
            var stepValue = random.Next(1, 5);
            var template = templates[random.Next(templates.Length)];
            var newCode = string.Format(template, variable.Label, startValue, "n", stepValue);
        
            code.ShiftLines(4);
            code.Insert(0, newCode);
            code.Append("}\n");

            variable.IsBounded = true;
        }

        public bool TryGetComplexity(Complexity oldComplexity, out Complexity newComplexity)
        {
            newComplexity = complexities.ContainsKey(oldComplexity)
                ? complexities[oldComplexity]
                : oldComplexity;
            return complexities.ContainsKey(oldComplexity);
        }
    }
}