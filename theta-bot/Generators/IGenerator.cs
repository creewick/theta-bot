using theta_bot.Classes;

namespace theta_bot.Generators
{
    public interface IGenerator
    {
        Exercise Generate(Exercise exercise, params Tag[] tags);
    }
}