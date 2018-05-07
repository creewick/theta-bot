using System;
using System.Text;
using theta_bot.Classes;

namespace theta_bot.Generators
{
    public interface IGenerator
    {
        void ChangeCode(StringBuilder code, Func<Variable> getNextVar, Random random);
        bool TryGetComplexity(Complexity oldComplexity, out Complexity newComplexity);
    }
}