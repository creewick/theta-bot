using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace theta_bot
{
    public class Exercise
    {
        private readonly StringBuilder Code = new StringBuilder();
        private readonly List<Variable> UsedVars = new List<Variable>();
        public Complexity Complexity { get; private set; } = Complexity.Constant;

        public string GetMessage() => $"Найдите сложность алгоритма:\n\n```\n{Code}\n```\n\n";
        public string GetCode() => Code.ToString();
       
        public Exercise Generate(Generator generator, Random random)
        {
            generator.ChangeCode(Code, UsedVars, random);
            Complexity = generator.GetComplexity(Complexity);
            return this;
        }
        
        public Exercise BoundVars()
        {
            foreach (var e in UsedVars)
                if (!e.IsBounded)
                {
                    Code.Insert(0, $"var {e.Label} = 0;\n");
                    e.SetBound(true);
                }
            return this;
        }

        public IEnumerable<string> GetOptions(Random random, int count)
        {
            return Complexity.All
                .Where(c => !Equals(c, Complexity))
                .Shuffle(random)
                .Take(count-1)
                .Append(Complexity)
                .Shuffle(random)
                .Select(c => c.Value);
        }
    }
}