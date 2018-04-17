using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace theta_bot
{
    public class Exercise
    {
        private readonly StringBuilder Code = new StringBuilder();
        private readonly List<Variable> UsedVars = new List<Variable>();
        public Complexity Complexity { get; private set; } = Complexity.Constant;
        public string Message => Code.ToString();
       
        public Exercise Generate(IGenerator generator, Random random)
        {
            if (generator.TryGetComplexity(Complexity, out var newComplexity))
            {
                Complexity = newComplexity;
                generator.ChangeCode(Code, () => GetNextVar(random), random);
            }
            return this;
        }
        
        #region Variables
        private static readonly string[] Labels = {"a", "b", "c", "i", "j", "k"};
        
        private Variable GetNextVar(Random random)
        {
            return UsedVars.Count(v => !v.IsBounded) > 0 && random.Next(2) == 1
                ? GetOldVar(random)
                : GetNewVar(random);
        }

        private Variable GetOldVar(Random random) => 
            UsedVars
                .Where(v => !v.IsBounded)
                .Shuffle(random)
                .First();

        private Variable GetNewVar(Random random)
        {
            var label = Labels
                .Where(l => !UsedVars.Select(v => v.Label).Contains(l))
                .Shuffle(random)
                .First();
            UsedVars.Add(new Variable(label));
            return UsedVars.Last();
        }
        
        public Exercise BoundVars()
        {
            foreach (var v in UsedVars.Where(v => !v.IsBounded))
                {
                    Code.Insert(0, $"var {v.Label} = 0;\n");
                    v.IsBounded = true;
                }
            Code.Insert(0, "var count = 0;\n");
            return this;
        }
        #endregion

        public IEnumerable<string> GetOptions(Random random, int count) => 
            Complexity.All
                .Where(c => !Equals(c, Complexity))
                .Shuffle(random)
                .Take(count-1)
                .Append(Complexity)
                .Shuffle(random)
                .Select(c => c.Value);
    }
}