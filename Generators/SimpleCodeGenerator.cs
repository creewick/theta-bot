using System;
using System.Collections.Generic;
using System.Text;

namespace theta_bot.Generators
{
    public class SimpleCodeGenerator : IGenerator
    {
        private readonly string[] labels = {"i", "n", "k"};
        private readonly string[] templates =
        {
            "if ({0} % {1} == 0) count++;",
            "if ({0} < {1}) count++;",
            "if ({0} > {1}) count++;",
            "count++;"
        };

        public void ChangeCode(StringBuilder code, List<Variable> vars)
        {
            var random = new Random();
            var label = labels[random.Next(labels.Length)];
            var number = random.Next(10);
            var template = templates[random.Next(templates.Length)];
            
            code.AppendLine(string.Format(template, label, number));
            vars.Add(new Variable(label, false));
        }

        public Complexity GetComplexity(Complexity complexity) => complexity;
    }
}