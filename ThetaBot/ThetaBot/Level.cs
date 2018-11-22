using System.Collections.Generic;

namespace ThetaBot
{
    public class Level
    {
        public readonly string Id;
        public readonly List<Generator> Generators;

        public Level(string id, List<Generator> generators)
        {
            Id = id;
            Generators = generators;
        }
    }

    public abstract class Generator
    {
        public readonly string Id;
        public readonly int MaxPoints;
        public readonly string Answer;

        public abstract Exercise GetExercise();

        protected Generator(int maxPoints, string answer, string id)
        {
            MaxPoints = maxPoints;
            Answer = answer;
            Id = id;
        }
    }

    public class Exercise
    {
        public readonly string Text;
        public readonly string Answer;
        public readonly List<string> Variants;

        public Exercise(string text, string answer, List<string> variants)
        {
            Text = text;
            Answer = answer;
            Variants = variants;
        }
    }
}
