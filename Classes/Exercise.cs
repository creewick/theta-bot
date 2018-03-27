using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using theta_bot.Generators;

namespace theta_bot
{
    public class Exercise
    {
        public readonly StringBuilder Code = new StringBuilder();
        public readonly List<Variable> Vars = new List<Variable>();
        public Complexity Complexity = Complexity.Constant;
        private const string Title = "Найдите сложность алгоритма: \n\n";

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
        
        public string GetMessage()
        {
            Code.Insert(0, Title);
            var str = Code.ToString();
            Code.Remove(0, Title.Length);
            return str;
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