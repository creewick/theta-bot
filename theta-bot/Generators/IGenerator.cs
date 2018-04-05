using System;
using System.Text;

namespace theta_bot
{
    public interface IGenerator
    {
        void ChangeCode(StringBuilder code, Func<Variable> getNextVar, Random random);
        bool TryGetComplexity(Complexity oldComplexity, out Complexity newComplexity);
    }
}