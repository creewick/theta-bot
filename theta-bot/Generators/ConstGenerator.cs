using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using theta_bot.Classes;
using theta_bot.Extentions;

namespace theta_bot.Generators
{
    public class ConstGenerator : Generator
    {       
        private static readonly string[] CodeTemplates =
        {
            "{0}+=1;\n",
            "{0}++;\n"
        };
        
        private static readonly string[] ForTemplates =
        {
            "for (var {0}={1}; {0}<{2}; {0}+={3})\n{{\n",
            "for (var {0}={1}; {0}<{2}; {0}={0}+{3})\n{{\n",
            "for (var {0}={2}; {0}>{1}; {0}-={3})\n{{\n",
            "for (var {0}={2}; {0}>{1}; {0}={0}-{3})\n{{\n",
        };

        private static readonly string[] WhileTemplates =
        {
            "var {0} = {1};\nwhile ({0} < {2})\n{{\n    {0} += {3};\n",
            "var {0} = {1};\nwhile ({0} < {2})\n{{\n    {0} = {0} + {3};\n",
            "var {0} = {2};\nwhile ({0} < {1})\n{{\n    {0} -= {3};\n",
            "var {0} = {2};\nwhile ({0} > {1})\n{{\n    {0} = {0} - {3};\n",
        };

        private static readonly Dictionary<Tag, string[]> Templates = 
            new Dictionary<Tag, string[]>
            {
                {Tag.Code, CodeTemplates},
                {Tag.For, ForTemplates},
                {Tag.While, WhileTemplates}
            };
        
        public override Exercise Generate(Exercise exercise, Random random, params Tag[] tags)
        {
            var complexity = GetComplexity(exercise, tags);
            if (complexity == null) return exercise;
            
            var codeType = CodeType(tags);

            var newVar = (codeType == Tag.Code)
                ? new Variable(false, "count")
                : new Variable(true);
            
            var start = new Variable(random.Next(2));
            var end = new Variable(random.Next(1, 5) * 100);
            var step = new Variable(random.Next(1, 5));
            var template = Templates[codeType].Random();
            
            var newCode = string.Format(template, newVar, start, end, step);
            var newTags = new List<Tag> {codeType};
            var newVars = new List<Variable> {newVar, start, end, step};

            if (tags.Contains(Tag.DependFromValue))
            {
                exercise = exercise.ReplaceVar(exercise.Vars[2], newVar);
                newTags.Add(Tag.DependFromValue);
            }

            if (tags.Contains(Tag.DependFromStep))
            {
                exercise = exercise.ReplaceVar(exercise.Vars[3], newVar);
                newTags.Add(Tag.DependFromStep);
            }
            
            var previous = (codeType == Tag.Code)
                ? exercise.Previous
                : exercise;
            return new Exercise(newVars, newCode, newTags, (Complexity)complexity, previous);
        }

        private static Complexity? GetComplexity(Exercise previous, Tag[] tags)
        {
            if (!Depend(tags))
                return Complexity.Const;
            if (previous.Complexity == Complexity.Const)
                return Complexity.Const;
            if (previous.Complexity == Complexity.Log) 
                return tags.Contains(Tag.DependFromValue) 
                    ? Complexity.Const 
                    : Complexity.Log;
            if (previous.Complexity == Complexity.Linear)
                return tags.Contains(Tag.DependFromValue)
                    ? Complexity.Const
                    : Complexity.Linear;
            return null;
        }
    }

    [TestFixture]
    public class TestConst
    {
        [Test]
        public void Test1()
        {
            Console.WriteLine(
                new Exercise()
                    .Generate(new ConstGenerator(), Tag.Code)
                    .Generate(new ConstGenerator(), Tag.For)
                    .Generate(new ConstGenerator(), Tag.For, Tag.DependFromValue)
                    .Build());
        }
    }
}