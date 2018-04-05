using System;
using System.Text;

namespace theta_bot
{
    public class SimpleWhileLoop : IGenerator
    {
        private readonly string[] templates =
        {
            "var {0} = {1};\nwhile({0} < {2})\n{{\n    {0}++;\n",
            "var {0} = {1};\nwhile({0} < {2})\n{{\n    {0}+={3};\n",
            "var {0} = {1};\nwhile({0} < {2})\n{{\n    {0} = {0} + {3};\n",
        };
        
        public void ChangeCode(StringBuilder code, Func<Variable> getNextVar, Random random)
        {
            var variable = getNextVar();
            var startValue = random.Next(2);
            var endValue = random.Next(1, 5) * 1000;
            var stepValue = random.Next(1, 5);
            var template = templates[random.Next(templates.Length)];
            var newCode = string.Format(template, variable.Label, startValue, endValue, stepValue);
            
            code.ShiftLines(4);
            code.Insert(0, newCode);
            code.Append("}\n");

            variable.IsBounded = true;
        }

        public bool TryGetComplexity(Complexity oldComplexity, out Complexity newComplexity)
        {
            newComplexity = oldComplexity;
            return true;
        }
    }
}