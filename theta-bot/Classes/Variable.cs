using System;

namespace theta_bot.Classes
{
    public class Variable
    {
        private static int LastID;
        
        public readonly int ID;
        public readonly string Value;
        public bool IsBounded { get; private set; }

        public Variable(bool bound=false, string value=null)
        {
            ID = LastID++;
            IsBounded = bound;
            Value = value;
        }

        public Variable(int number)
        {
            ID = LastID++;
            IsBounded = true;
            Value = number.ToString();
        }

        public void Bound() => IsBounded = true;
        
        public override string ToString() => $"%{ID}%";
    }
}