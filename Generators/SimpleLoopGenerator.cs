using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace theta_bot.Generators
{
    public class SimpleLoopGenerator : IGenerator
    {
        private readonly Dictionary<Complexity, Complexity> complexity = 
            new Dictionary<Complexity, Complexity>
        {
            {Complexity.Constant, Complexity.Linear},
            {Complexity.Linear, Complexity.Quadratic},
            {Complexity.Quadratic, Complexity.Cubic},
            {Complexity.Logarithmic, Complexity.Polynomial}
        };
        
        private readonly string[] templates =
        {
            "for (var {0} = 0; {0} < {2}; {0}++)\n",
            "for (var {0} = 0; {0} < {2}; {0} += {3})\n"
        };
        
        public void ChangeCode(StringBuilder code, List<Variable> vars)
        {
            var random = new Random();
            
            var variable = vars
                .Where(v => !v.IsBounded)
                .Shuffle(random)
                .First();
            
            var startValue = random.Next(2);
            var endValue = random.Next(1, 5) * 1000;
            var stepValue = random.Next(1, 5);
            var template = templates[random.Next(templates.Length)];
            var newCode = string.Format(template, variable.Label, startValue, endValue, stepValue);
            
            code.ShiftLines(4);
            code.Insert(0, newCode);
            code.Insert(newCode.Length, "{\n");
            code.Append("}");
            
            variable.IsBounded = true;
        }

        public Complexity GetComplexity(Complexity oldComplexity) => 
            complexity[oldComplexity];
    }
}