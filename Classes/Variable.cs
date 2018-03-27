namespace theta_bot
{
    public class Variable
    {
        public readonly string Label;
        public bool IsBounded;

        public Variable(string label, bool bound)
        {
            Label = label;
            IsBounded = bound;
        }
    }
}