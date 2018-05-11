using System.Collections.Generic;
using System.Text;
using theta_bot.Extentions;
using theta_bot.NewGenerators;

namespace theta_bot.Classes
{
    public class Exercise
    {
        public readonly Complexity Complexity;
        public readonly List<Variable> Vars;
        public readonly StringBuilder Code;
        public readonly Variable MainVar;
        public readonly List<Tag> Tags;
        private int nextVarIndex;

        public Exercise()
        {
            MainVar = new Variable("n");
            Complexity = new Complexity(0, 0);
            Vars = new List<Variable>{MainVar};
            Code = new StringBuilder();
            Tags = new List<Tag>();
            nextVarIndex = Vars.Count;
        }
        
        public Exercise(Exercise previous, Complexity complexity=null, List<Variable> vars=null, 
            Variable mainVar=null, StringBuilder code=null, List<Tag> tags=null)
        {
            MainVar = mainVar ?? previous.MainVar;
            Complexity = complexity ?? previous.Complexity;
            Vars = vars ?? previous.Vars;
            Code = code ?? previous.Code;
            Tags = tags ?? previous.Tags;
            nextVarIndex = Vars.Count;
        }

        public override string ToString() => BoundVars().PackVars().Code.ToString();
        
        public Exercise Generate(INewGenerator generator, params Tag[] desiredTags) =>
            generator.Generate(this, desiredTags);
        
        public Variable NextVar(bool bound=false) => new Variable($"{nextVarIndex++}", bound);

        public Exercise RenameVar(Variable variable, string label)
        {
            var oldLabel = variable.Label;
            variable.Rename(label);
            Code.Replace(oldLabel, variable.Label);
            return this;
        }

        private Exercise BoundVars()
        {
            var exercise = new Exercise(this);
            foreach (var variable in exercise.Vars)
                if (!variable.IsBounded && variable != MainVar)
                {
                    exercise.Code.Insert(0, $"{variable.Label} = 0;\n");
                    variable.IsBounded = true;
                }
            return exercise;
        }
        
        private static readonly List<string> Labels = new List<string>{"a", "b", "c", "i", "j", "k", "m"};

        private Exercise PackVars()
        {
            var exercise = new Exercise(this);
            foreach (var v in exercise.Vars)
            {
                if (v.Label == "%count%")
                    exercise.Code.Replace(v.Label, "count");
                else if (v == MainVar)
                    exercise.Code.Replace(v.Label, "n");
                else
                {
                    var newLabel = Labels.Random();
                    Labels.Remove(newLabel);
                    exercise.Code.Replace(v.Label, newLabel);
                }
            }

            exercise.Vars.Clear();
            return exercise;
        }
    }
}