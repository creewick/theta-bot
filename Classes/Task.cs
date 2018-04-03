using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace theta_bot
{
    public class Task
    {
        private readonly StringBuilder Code = new StringBuilder();
        private readonly List<Variable> UsedVars = new List<Variable>();
        public Complexity Complexity { get; private set; } = Complexity.Constant;

        public string GetMessage() => $"```\nНайдите сложность алгоритма:\n\n{Code}\n```";
       
        public Task Generate(Generator generator, Random random)
        {
            if (generator.TryGetComplexity(Complexity, out var newComplexity))
            {
                Complexity = newComplexity;
                generator.ChangeCode(Code, UsedVars, random);
            }
            return this;
        }
        
        public Task BoundVars()
        {
            foreach (var e in UsedVars)
                if (!e.IsBounded)
                {
                    Code.Insert(0, $"var {e.Label} = 0;\n");
                    e.IsBounded = true;
                }
            Code.Insert(0, "var count = 0;\n");
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