using theta_bot.Classes;

namespace theta_bot.Generators
{
    public interface INewGenerator
    {
        Exercise Generate(Exercise exercise, params Tag[] tags);
    }
}