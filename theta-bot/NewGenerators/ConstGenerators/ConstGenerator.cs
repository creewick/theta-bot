using System.Collections.Generic;
using theta_bot.Classes;

namespace theta_bot.NewGenerators.ConstGenerators
{
    public class ConstGenerator : INewGenerator
    {
        private static readonly Dictionary<Tag, INewGenerator> Generators = 
            new Dictionary<Tag, INewGenerator>
        {
            {Tag.Code, new ConstCodeGenerator()},
            {Tag.For, new ConstCodeGenerator()},
            {Tag.While, new ConstWhileGenerator()}
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