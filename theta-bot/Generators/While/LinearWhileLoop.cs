using System;
using System.Collections.Generic;
using System.Text;
using theta_bot.Classes;
using theta_bot.Extentions;

namespace theta_bot
{
    public class LinearWhileLoop: ICycleGenerator
    {
        private readonly Dictionary<Complexity, Complexity> complexities = 
            new Dictionary<Complexity, Complexity>
            {
                {Complexity.Constant, Complexity.N},
                {Complexity.LogN, Complexity.NLogN},
                {Complexity.N, Complexity.N2},
                {Complexity.N2, Complexity.N3}
            };
        
        private readonly string[] templates =
        {
            "var {0} = {1};\nwhile ({0} < {2} / 2)\n{{\n    {0}++;\n",
            "var {0} = {1};\nwhile ({0} < {2})\n{{\n    {0}+={3};\n",
            "var {0} = {1};\nwhile ({0} < {2})\n{{\n    {0} = {0} + {3};\n",
        };
        
        public void ChangeCode(StringBuilder code, Func<Variable> getNextVar, Random random) => 
            AddCycle("n", code, getNextVar, random);

        public void AddCycle(string cycleVar, StringBuilder code, Func<Variable> getNextVar, Random random)
        {
            var variable = getNextVar();
            var startValue = random.Next(2);
            var stepValue = random.Next(1, 5);
            var template = templates[random.Next(templates.Length)];
            var newCode = string.Format(template, variable.Label, startValue, cycleVar, stepValue);
        
            code.Indent(4);
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