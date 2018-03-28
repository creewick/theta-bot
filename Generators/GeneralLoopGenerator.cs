using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace theta_bot.Generators
{
    public class GeneralLoopGenerator: Generator
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
            "for (var {0}=0; {0}<{2}; {0}++)\n",
            "for (var {0}=0; {0}<{2}; {0}+={3})\n"
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
            code.Insert(newCode.Length, "{\n");
            code.Append("}");
        
            variable.SetBound(true);
        }

        public override Complexity GetComplexity(Complexity oldComplexity) => complexities[oldComplexity];
    }
}