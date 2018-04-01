using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace theta_bot.Generators
{
    public class SimpleLoopGenerator : Generator
    {
        private readonly string[] templates =
        {
            "for (var {0}=0; {0}<{2}; {0}++)\n",
            "for (var {0}=0; {0}<{2}; {0}+={3})\n",
            "for (var {0}=0; {0}<{2}; {0}={0}+{3})\n"
        };
        
        public override void ChangeCode(StringBuilder code, List<Variable> vars, Random random)
        {
            var variable = GetNextVar(vars, random);
            
            var startValue = random.Next(2);
            var endValue = random.Next(1, 5) * 1000;
            var stepValue = random.Next(1, 5);
            var template = templates[random.Next(templates.Length)];
            var newCode = string.Format(template, variable.Label, startValue, endValue, stepValue);
            
            code.ShiftLines(4);
            code.Insert(0, newCode);
            code.Insert(newCode.Length, "{\n");
            code.Append("}\n");
            
            variable.IsBounded = true;
        }

        public override bool TryGetComplexity(Complexity oldComplexity, out Complexity newComplexity)
        {
            newComplexity = oldComplexity;
            return true;
        }
    }
}