namespace theta_bot
{
    public struct Variable
    {
        public readonly string Label;
        public bool IsBounded { get; private set; }

        public Variable(string label, bool bound=false)
        {
            Label = label;
            IsBounded = bound;
        }

        public void SetBound(bool bound)
        {
            IsBounded = bound;
        }
    }
}