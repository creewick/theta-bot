namespace theta_bot.Classes
{
    public class Variable
    {
        public readonly string Label;
        public bool IsBounded;

        public Variable(string label, bool bound=false)
        {
            IsBounded = bound;
        }
    }
}