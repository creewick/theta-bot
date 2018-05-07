using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using theta_bot.Classes;
using theta_bot.Extentions;

namespace theta_bot.NewGenerators
{
    public static class ConstForGenerator
    {
        private static readonly string[] templates =
        {
            "for (var {0}={1}; {0}<{2}; {0}++)\n",
            "for (var {0}={1}; {0}<{2}; {0}+={3})\n",
            "for (var {0}={1}; {0}<{2}; {0}={0}+{3})\n",
            "for (var {0}={2}; {0}>{1}; {0}--)\n",
            "for (var {0}={2}; {0}>{1}; {0}-={3})\n",
            "for (var {0}={2}; {0}>{1}; {0}={0}-{3})\n",
        };

        public static Exercise Generate(Exercise previous)
        {
            var code = new StringBuilder(previous.ToString());
            var newTags = new List<Tag>(previous.Tags).Append(Tag.Code).ToList();

            var variable = previous.GetNextVar();
            variable.IsBounded = true;
            var start = new Random().Next(2);
            var end = new Random().Next(1, 5) * 1000;
            var step = new Random().Next(1, 5);
            var template = templates[new Random().Next(templates.Length)];
            var cycleCode = string.Format(template, variable.Label, start, end, step);

            code.Indent(4);
            code.Insert(0, cycleCode);
            code.Insert(cycleCode.Length, "{\n");
            code.Append("}\n");
            
            return new Exercise(previous.Complexity, );
        }
        
        public void ChangeCode(StringBuilder code, Func<Variable> getNextVar, Random random) => 
            AddCycle(null, code, getNextVar, random);

        public bool TryGetComplexity(Complexity oldComplexity, out Complexity newComplexity)
        {
            newComplexity = oldComplexity;
            return true;
        }

        public void AddCycle(string cycleVar, StringBuilder code, Func<Variable> getNextVar, Random random)
        {
            var variable = getNextVar();
            var startValue = random.Next(2);
            var endValue = random.Next(1, 5) * 1000;
            var stepValue = random.Next(1, 5);
            var template = templates[random.Next(templates.Length)];
            var newCode = string.Format(template, variable.Label, startValue, endValue, stepValue);
            
            code.Indent(4);
            code.Insert(0, newCode);
            code.Insert(newCode.Length, "{\n");
            code.Append("}\n");
            
            variable.IsBounded = true;
        }
    }
}