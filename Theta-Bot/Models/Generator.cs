namespace Theta_Bot
{
    public class Generator
    {
        public readonly string Id;
        public readonly string Answer;
        public readonly int MaxPoints;

        public Generator(int maxPoints, string answer, string id)
        {
            MaxPoints = maxPoints;
            Answer = answer;
            Id = id;
        }
    }
}
