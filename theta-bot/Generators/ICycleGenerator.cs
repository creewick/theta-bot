using System;
using System.Text;
using theta_bot.Classes;
using theta_bot.Generators;

namespace theta_bot
{
    public interface ICycleGenerator : IGenerator
    {
        void AddCycle(string cycleVar, StringBuilder code, Func<Variable> getNextVar, Random random);
    }
}