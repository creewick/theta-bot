using System;
using System.Collections.Generic;
using System.Text;

namespace theta_bot.Generators
{
    public class LogarithmicLoopGenerator : Generator
    {
        private readonly Dictionary<Complexity, Complexity> complexities = 
            new Dictionary<Complexity, Complexity>
            {
                {Complexity.Constant, Complexity.Logarithmic},
                {Complexity.Logarithmic, Complexity.Polynomial}
            };
        
        private readonly string[] templates =
        {
            "for (var {0}={1}; {0}<{2}; {0}*={3})\n",
            "for (var {0}={1}; {0}<{2}*{2}; {0}*={3})\n",
            "for (var {0}={1}; {0}<{2}; {0}={0}*{3})\n",
            "for (var {0}={2}; {0}>{1}; {0}/={3})\n",
            "for (var {0}={2}*{2}; {0}>{1}; {0}/={3})\n",
            "for (var {0}={2}; {0}>{1}; {0}={0}/{3})\n"
        };
        
        public override void ChangeCode(StringBuilder code, List<Variable> vars, Random random)
        {
            var variable = GetNextVar(vars, random);
            var startValue = random.Next(2);
            var stepValue = random.Next(2, 5);
            var template = templates[random.Next(templates.Length)];
            var newCode = string.Format(template, variable.Label, startValue, "n", stepValue);
        
            code.ShiftLines(4);
            code.Insert(0, newCode);
            code.Insert(newCode.Length, "{\n");
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