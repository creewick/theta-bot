using System.Linq;
using theta_bot.Classes;

namespace theta_bot.NewGenerators
{
    public class ConstGenerator : IGenerator2
    {        
        public Exercise Generate(Exercise previous, params Tag[] desiredTags)
        {
            var tags = desiredTags.ToList();
            if (tags.Contains(Tag.Code))
                return ConstCodeGenerator.Generate(previous);
            if (tags.Contains(Tag.For))
                return ConstForGenerator.Generate(previous);
            return previous;
        }
    }
}