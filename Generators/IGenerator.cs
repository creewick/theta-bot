using System.Collections.Generic;
using System.Text;

namespace theta_bot
{
    public interface IGenerator
    {
        void ChangeCode(StringBuilder code, List<Variable> vars);
        Complexity GetComplexity(Complexity oldComplexity);
    }
}