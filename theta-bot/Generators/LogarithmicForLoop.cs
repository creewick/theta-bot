using System;
using System.Collections.Generic;
using System.Text;

namespace theta_bot
{
    public class LogarithmicForLoop : IGenerator
    {
        private readonly Dictionary<Complexity, Complexity> complexities = 
            new Dictionary<Complexity, Complexity>
            {
                {Complexity.Constant, Complexity.Logarithmic},
                {Complexity.Logarithmic, Complexity.SquaredLogarithm}
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
        
        public void ChangeCode(StringBuilder code, Func<Variable> getNextVar, Random random)
        {
            var variable = getNextVar();
            var startValue = random.Next(1, 3);
            var stepValue = random.Next(2, 5);
            var template = templates[random.Next(templates.Length)];
            var newCode = string.Format(template, variable.Label, startValue, "n", stepValue);
        
            code.ShiftLines(4);
            code.Insert(0, newCode);
            code.Insert(newCode.Length, "{\n");
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