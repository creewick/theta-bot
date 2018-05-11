using System.Collections.Generic;
using theta_bot.Classes;
using theta_bot.NewGenerators.ConstGenerators;

namespace theta_bot.NewGenerators.LogGenerators
{
    public class LogGenerator : INewGenerator
    {
        private static readonly Dictionary<Tag, INewGenerator> Generators = 
            new Dictionary<Tag, INewGenerator>
            {
                {Tag.For, new LogForGenerator()}
            };
        
        public Exercise Generate(Exercise exercise, params Tag[] desiredTags)
        {
            foreach (var tag in desiredTags)
                if (Generators.ContainsKey(tag))
                    exercise = Generators[tag].Generate(exercise, desiredTags);
            return exercise;
        }
    }
}