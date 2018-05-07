using System;
using System.Text;
using theta_bot.Classes;
using theta_bot.Extentions;

namespace theta_bot
{
    public static class Series
    {
        public static readonly string[] ArithmeticTemplates =
        {
            "for (var {0}={1}; {0}<{2}; {0}++)\n",
            "for (var {0}={1}; {0}<{2}; {0}+={3})\n",
            "for (var {0}={1}; {0}<{2}; {0}={0}+{3})\n",
            "for (var {0}={2}; {0}>{1}; {0}-={3})\n",
            "for (var {0}={2}; {0}>{1}; {0}={0}-{3})\n"
        };

        public static readonly string[] GeometricTemplates =
        {
            "for (var {0}={1}; {0}<{2}; {0}*={3})\n",
            "for (var {0}={1}; {0}<{2}; {0}={0}*{3})\n",
            "for (var {0}={2}; {0}>{1}; {0}/={3})\n",
            "for (var {0}={2}; {0}>{1}; {0}={0}/{3})\n"
        };
        
        public static void ChangeCode(StringBuilder code, Func<Variable> getNextVar, Random random, 
            ICycleGenerator cycleGenerator, string[] templates)
        {
            var variable = getNextVar();
            variable.IsBounded = true;
            
            var newCode = new StringBuilder();
            new SimpleCodeBlock().ChangeCode(newCode, getNextVar, random);
            cycleGenerator.AddCycle(variable.Label, newCode, getNextVar, random);
            
            var template = templates[random.Next(templates.Length)];
            var outerCycle = string.Format(template, variable.Label, 0, "n", 1);
            
            newCode.Indent(4);
            newCode.Insert(0, outerCycle);
            newCode.Insert(outerCycle.Length, "{\n");
            newCode.Append("}\n");
            code.Append(newCode);
        }
    }
}