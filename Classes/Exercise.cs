using System;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace theta_bot
{
    public class Exercise
    {
        private readonly StringBuilder Code = new StringBuilder();
        private readonly List<Variable> Vars = new List<Variable>();
        private Complexity Complexity { get; set; } = Complexity.Constant;

        public string GetMessage() => $"Найдите сложность алгоритма: ```\n{Code}```";
        
        public Exercise Generate(IGenerator generator)
        {
            generator.ChangeCode(Code, Vars);
            Complexity = generator.GetComplexity(Complexity);
            return this;
        }

        public Exercise BoundVars()
        {
            foreach (var e in Vars)
                if (!e.IsBounded)
                {
                    Code.Insert(0, $"var {e.Label} = 0;\n");
                    e.IsBounded = true;
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