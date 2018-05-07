using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using theta_bot.Extentions;
using theta_bot.Generators;
using theta_bot.NewGenerators;

namespace theta_bot.Classes
{
    public class Exercise
    {
        public readonly Complexity Complexity = new Complexity(0, 0);
        public readonly List<Variable> Vars = new List<Variable>();
        public readonly StringBuilder Code = new StringBuilder();
        public readonly List<Tag> Tags = new List<Tag>();

        public Exercise(){}
        
        public Exercise(Complexity complexity, List<Variable> vars, StringBuilder code, List<Tag> tags)
        {
            Complexity = complexity;
            Vars = vars;
            Code = code;
            Tags = tags;
        }
        
        public override string ToString() => ToString(0);
        public string ToString(int indent) => Code.Indent(indent).ToString();
        
        public Exercise Generate(IGenerator2 generator, params Tag[] desiredTags) =>
            generator.Generate(this, desiredTags);
        
        #region Variables
        private static readonly string[] Labels = {"a", "b", "c", "i", "j", "k"};
        
        public Variable GetNextVar() => 
            Vars.Count(v => !v.IsBounded) > 0 && new Random().Next(2) == 1
                ? GetOldVar()
                : GetNewVar();

        private Variable GetOldVar() => 
            Vars
                .Where(v => !v.IsBounded)
                .Shuffle()
                .First();

        private Variable GetNewVar()
        {
            var label = Labels
                .Where(l => !Vars.Select(v => v.Label).Contains(l))
                .Shuffle()
                .First();
            Vars.Add(new Variable(label));
            return Vars.Last();
        }
        
        public Exercise BoundVars()
        {
            foreach (var v in Vars.Where(v => !v.IsBounded))
            {
                Code.Insert(0, $"var {v.Label} = 0;\n");
                v.IsBounded = true;
            }
            Code.Insert(0, "var count = 0;\n");
            return this;
        }
        #endregion
    }
}