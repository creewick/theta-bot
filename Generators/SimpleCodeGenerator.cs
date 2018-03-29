using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace theta_bot.Generators
{
    public class SimpleCodeGenerator : Generator
    {
        private readonly string[] templates =
        {
            "if ({0} % {1} == 0) count++;",
            "if ({0} < {1}) count++;",
            "if ({0} > {1}) count++;",
            "count++;"
        };

        public override void ChangeCode(StringBuilder code, List<Variable> vars, Random random)
        {
            var variable = GetNextVar(vars, random);
            var number = random.Next(10);
            var template = templates[random.Next(templates.Length)];
            
            code.AppendLine(string.Format(template, variable.Label, number));
        }
        
        public override bool TryGetComplexity(Complexity oldComplexity, out Complexity newComplexity)
        {
            newComplexity = oldComplexity;
            return true;
        }
    }
}