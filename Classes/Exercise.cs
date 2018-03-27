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
        private readonly ComplexityInfo complexityInfo = new ComplexityInfo(Complexity.Constant);
        
        public string GetMessage() => $"Найдите сложность алгоритма: \n```\n{Code}```";
        public Complexity GetComplexity() => complexityInfo.Value;
        
        public Exercise Generate(IGenerator generator)
        {
            generator.ChangeCode(Code, Vars);
            generator.ChangeComplexity(complexityInfo);
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
            return ComplexityInfo.All.Keys
                .Where(c => !Equals(c, complexityInfo.Value))
                .Shuffle(random)
                .Take(count-1)
                .Append(complexityInfo.Value)
                .Shuffle(random)
                .Select(c => ComplexityInfo.All[c]);
        }
    }
}