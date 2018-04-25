using System;
using System.Text;

namespace theta_bot
{
    public interface ICycleGenerator : IGenerator
    {
        void AddCycle(string cycleVar, StringBuilder code, Func<Variable> getNextVar, Random random);
    }
}