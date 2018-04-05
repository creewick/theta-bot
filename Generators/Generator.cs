using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace theta_bot
{
    public abstract class Generator
    {
        public abstract void ChangeCode(StringBuilder code, List<Variable> vars, Random random);
        public abstract bool TryGetComplexity(Complexity oldComplexity, out Complexity newComplexity);
               
        protected static Variable GetNextVar(List<Variable> vars, Random random)
        {
            if (vars.Count(v => !v.IsBounded) > 0 && random.Next(2) == 1)
                return GetOldVar(vars, random);
            return GetNewVar(vars, random);
        }

        private static Variable GetOldVar(IEnumerable<Variable> vars, Random random) => 
            vars.Where(v => !v.IsBounded)
                .Shuffle(random)
                .First();

        private static readonly string[] labels = {"a", "b", "c", "i", "j", "k"};
        
        private static Variable GetNewVar(List<Variable> vars, Random random)
        {
            var usedLabels = vars
                .Select(v => v.Label);
            var label = labels
                .Where(l => !usedLabels.Contains(l))
                .Shuffle(random)
                .First();
            var variable = new Variable(label);
            vars.Add(variable);
            return variable;
        }
    }

    [TestFixture]
    public class Testss
    {
        [Test]
        public void Test()
        {
            
        }
    }
}