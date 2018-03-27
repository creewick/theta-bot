using System;
using System.CodeDom;
using System.Runtime.CompilerServices;
using System.Text;

namespace theta_bot.Generators
{
    public class SimpleCodeGenerator : Generator
    {
        private readonly string[] labels = {"i", "n", "k"};
        private readonly string[] templates =
        {
            "if ({0} % {1} == 0) count++;",
            "if ({0} < {1}) count++;",
            "if ({0} > {1}) count++;",
            "count++;"
        };
        
        public Exercise Generate(Exercise exercise, Random random)
        {
            var label = labels[random.Next(labels.Length)];
            var number = random.Next(10);
            var template = templates[random.Next(templates.Length)];
            
            var code = string.Format(template, label, number);
            exercise.Code.AppendLine(code);
            exercise.Vars.Add(new Variable(label, false));
            return exercise;
        }

        protected override void ChangeCode(Exercise exercise, Random random)
        {
            throw new NotImplementedException();
        }

        public Complexity GetComplexity(Complexity previousComplexity) => previousComplexity;
    }
}