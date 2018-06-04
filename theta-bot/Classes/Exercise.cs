using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using theta_bot.Extentions;
using theta_bot.Generators;

namespace theta_bot.Classes
{
    public class Exercise
    {
        private readonly List<string> labels = new List<string>{"a", "b", "c", "i", "j", "k", "m"};
        
        
        public readonly Variable MainVar = new Variable(false, "n");
        public readonly IReadOnlyList<Variable> Vars;
        public readonly IReadOnlyList<Tag> Tags;
        public readonly Complexity Complexity;
        public readonly Exercise Previous;
        public readonly string Code;

        public Exercise()
        {
            Complexity = new Complexity();
            Vars = new List<Variable>();
            Tags = new List<Tag>();
            Code = "";
        }

        public Exercise(IReadOnlyList<Variable> vars, string code, IReadOnlyList<Tag> tags, 
            Complexity complexity, Exercise previous)
        {
            Complexity = complexity;
            Previous = previous;
            Vars = vars;
            Code = code;
            Tags = tags;
        }

        public Exercise ReplaceVar(Variable oldVar, Variable newVar)
        {
            var code = Code.Replace(oldVar.ToString(), newVar.ToString());

            var vars = Vars.ToList();
            var index = vars.IndexOf(oldVar);
            vars[index] = newVar;

            return new Exercise(vars, code, Tags, Complexity, Previous);
        }
        
        private static readonly Random Random = new Random();
        public Exercise Generate(Generator generator, params Tag[] desiredTags) =>
            generator.Generate(this, Random, desiredTags);

        public override string ToString() => Build().BoundVars().NameVars().Code;

        public Exercise Build()
        {
            var exercise = this;
            var stack = new Stack<Exercise>();
            stack.Push(this);
            var complexity = Complexity;
            while (exercise.Previous != null)
            {
                stack.Push(exercise.Previous);
                if (!exercise.Tags.Contains(Tag.DependFromStep) &&
                    !exercise.Tags.Contains(Tag.DependFromValue))
                complexity = new Complexity(
                    complexity.N + exercise.Previous.Complexity.N, 
                    complexity.LogN + exercise.Previous.Complexity.LogN);
                exercise = exercise.Previous;
            }

            var first = stack.Pop();
            var code = new StringBuilder(first.Code);
            while (stack.Count > 0)
            {
                exercise = stack.Pop();
                code.Indent(4)
                    .Insert(0, exercise.Code)
                    .Append("}\n");
            }

            return new Exercise(Vars, code.ToString(), Tags, complexity, Previous);
        }

        private Exercise BoundVars()
        {
            var code = new StringBuilder(Code);
            var exercise = this;
            while (exercise != null)
            {
                foreach (var v in exercise.Vars)
                    if (v.Value == "count" || v.Value == null && !v.IsBounded)
                    {
                        code.Insert(0, $"var {v} = 0;\n");
                        v.Bound();
                    }

                exercise = exercise.Previous;
            }
            return new Exercise(Vars, code.ToString(), Tags, Complexity, Previous);
        }
        
        private Exercise NameVars()
        {
            var code = new StringBuilder(Code);
            var exercise = this;
            while (exercise != null)
            {
                foreach (var v in exercise.Vars)
                    if (v.Value != null)
                        code = code.Replace(v.ToString(), v.Value);
                    else
                    {
                        var newLabel = labels.Random();
                        labels.Remove(newLabel);
                        code = code.Replace(v.ToString(), newLabel);
                    }
                exercise = exercise.Previous;
            }
            return new Exercise(Vars, code.ToString(), Tags, Complexity, Previous);
        }
       
        public IEnumerable<Complexity> GetOptions(int count) =>
            Enumerable
                .Range(1, count - 1)
                .Select(n => new Complexity(Random.Next(0, 3), Random.Next(0, 3)))
                .Append(Complexity)
                .Shuffle();
    }
}