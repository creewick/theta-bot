using System.Collections.Generic;

namespace ThetaBot.Levels
{
    public interface ILevel
    {
        string GetLevelName();
        Dictionary<string, int> GetPointsByGenerator();
        IGenerator GetGenerator(string generatorName);

    }
}
