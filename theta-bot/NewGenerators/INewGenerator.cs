using theta_bot.Classes;

namespace theta_bot.NewGenerators
{
    public interface INewGenerator
    {
        Exercise Generate(Exercise exercise, params Tag[] desiredTags);
    }
}