using System;

namespace theta_bot.Classes
{
    public class Variable
    {
        public string Label { get; private set; }
        public bool IsBounded;

        public Variable(string label, bool bound=false)
        {
            Label = label;
            IsBounded = bound;
        }

        public void Rename(string label) => Label = label;

        public override string ToString() => Label;
    }
}