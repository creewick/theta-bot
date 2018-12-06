using System.Collections.Generic;

namespace Theta_Bot
{
    public class Level
    {
        public readonly string Id;
        public readonly IReadOnlyList<Generator> Generators;

        public Level(string id, List<Generator> generators)
        {
            Id = id;
            Generators = generators;
        }
    }
}
