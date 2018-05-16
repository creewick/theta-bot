using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions.Execution;
using theta_bot.Extentions;
using theta_bot.Generators;
using theta_bot.NewGenerators;

namespace theta_bot.Classes
{
    public class Exercise
    {
        public Complexity Complexity { get; private set; }
        public List<Variable> Vars { get; private set; }
        public StringBuilder Code { get; private set; }
        public Variable MainVar { get; private set; }
        public List<Tag> Tags { get; private set; }
        private int nextVarIndex;

        public Exercise()
        {
            MainVar = new Variable("%n%");
            Complexity = new Complexity(0, 0);
            Vars = new List<Variable>{MainVar};
            Code = new StringBuilder();
            Tags = new List<Tag>();
            nextVarIndex = 0;
        }

        public Exercise Copy()
        {
            var exercise = new Exercise();
            
            exercise.Complexity = Complexity;
            exercise.Vars = Vars
                .Select(var => new Variable(var.Label, var.IsBounded))
                .ToList();
            exercise.Code = new StringBuilder(Code.ToString());
            exercise.MainVar = exercise.Vars.First(var => var.Label == MainVar.Label);
            exercise.Tags = new List<Tag>(Tags);
            exercise.nextVarIndex = Vars.Count;

            return exercise;
        }

        public override string ToString() => BoundVars().PackVars().Code.ToString();
        
        public Exercise Generate(INewGenerator generator, params Tag[] desiredTags) =>
            generator.Generate(this, desiredTags);
        
        public Variable NextVar(bool bound=false) => new Variable($"%{nextVarIndex++}%", bound);

        public Exercise RenameVar(Variable variable, string label)
        {
            var oldLabel = variable.Label;
            variable.Rename(label);
            Code.Replace(oldLabel, variable.Label);
            return this;
        }

        private Exercise BoundVars()
        {
            var exercise = Copy();
            foreach (var variable in exercise.Vars)
                if (!variable.IsBounded && variable != exercise.MainVar)
                {
                    exercise.Code.Insert(0, $"{variable.Label} = 0;\n");
                    variable.IsBounded = true;
                }
            return exercise;
        }
        
        private static readonly List<string> Labels = new List<string>{"a", "b", "c", "i", "j", "k", "m"};

        private Exercise PackVars()
        {
            var exercise = Copy();
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