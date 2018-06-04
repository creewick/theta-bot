namespace theta_bot.Classes
{
    public class Exp
    {
        public readonly string Code = "count++;";
        public readonly Complexity Complexity = Complexity.Const;
        public readonly Variable[] Vars = new Variable[0];
        
        public Exp Bind()
    }
}