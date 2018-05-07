using theta_bot.Classes;

namespace theta_bot.NewGenerators
{
    public interface IGenerator2
    {
        Exercise Generate(Exercise previous, params Tag[] desiredTags);
    }
}