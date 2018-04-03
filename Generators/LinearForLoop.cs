using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace theta_bot.Generators
{
    public class LinearForLoop: Generator
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
            "for (var {0}={1}; {0}<{2}; {0}++)\n{\n",
            "for (var {0}={1}; {0}<{2}; {0}+={3})\n{\n",
            "for (var {0}={1}; {0}<{2}; {0}={0}+{3})\n{\n",
            "for (var {0}={2}/5; {0}>{1}; {0}--)\n{\n",
            "for (var {0}={2}/10; {0}>{1}; {0}--)\n{\n",
            "for (var {0}={2}; {0}>{1}; {0}-={3})\n{\n",
            "for (var {0}={2}; {0}>{1}; {0}={0}-{3})\n{\n",
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
        
            variable.IsBounded = true;
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