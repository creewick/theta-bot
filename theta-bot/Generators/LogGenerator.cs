using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using theta_bot.Classes;
using theta_bot.Extentions;

namespace theta_bot.Generators
{
    public class LogGenerator : INewGenerator
    {
        private static readonly string[] ForTemplates =
        {
            "for (var {0}={1}; {0}<{2}; {0}*={3})\n{{\n",
            "for (var {0}={1}; {0}<{2}*{2}; {0}*={3})\n{{\n",
            "for (var {0}={1}; {0}<{2}; {0}={0}*{3})\n{{\n",
            "for (var {0}={2}; {0}>{1}; {0}/={3})\n{{\n",
            "for (var {0}={2}*{2}; {0}>{1}; {0}/={3})\n{{\n",
            "for (var {0}={2}; {0}>{1}; {0}={0}/{3})\n{{\n"
        };

        private static readonly string[] WhileTemplates =
        {
            "var {0} = {1};\nwhile ({0} < {2})\n{{\n    {0}*={3};\n",
            "var {0} = {1};\nwhile ({0} < {2})\n{{\n    {0} = {0} * {3};\n",
            "var {0} = {1};\nwhile ({0} < {2} * {2})\n{{\n    {0} = {0} * {3};\n",
        };
        
        private static readonly Dictionary<Tag, string[]> Templates = 
            new Dictionary<Tag, string[]>
            {
                {Tag.For, ForTemplates},
                {Tag.While, WhileTemplates}
            };
        
        public Exercise Generate(Exercise exercise, params Tag[] tags)
        {
            exercise = exercise.Copy();
            var mainTag = MainTag(tags);
            var depend = tags.Contains(Tag.Depend);
            
            var variable = depend
                ? exercise.MainVar
                : exercise.AddNewVar(true);
            var start = new Random().Next(1, 3);
            var step = new Random().Next(2, 5);
            var end = depend
                ? exercise.AddNewVar(true)
                : exercise.MainVar;
            var template = Templates[mainTag].Random();

            var code = string.Format(template, variable, start, end, step);
            exercise.Code.Indent(4);
            exercise.Code.Insert(0, code);
            exercise.Code.Append("}\n");
            
            exercise.Tags.Append(mainTag);
            if (depend) exercise.Tags.Append(Tag.Depend);

            exercise.MainVar = end;
            
            exercise.Complexity = depend
                ? null
                : new Complexity(
                    exercise.Complexity.N, 
                    exercise.Complexity.LogN + 1);

            return exercise;
        }
        
        private static Tag MainTag(params Tag[] tags)
        {
            if (tags.Contains(Tag.For))
                return Tag.For;
            if (tags.Contains(Tag.While))
                return Tag.While;
            throw new ArgumentException("Generator called without needed tag");
        }
    }
    
    [TestFixture]
    public class LogForShould
    {
        [Test]
        public void Test()
        {
            var a = new Exercise()
                .Generate(new ConstGenerator(), Tag.Code)
                .Generate(new LogGenerator(), Tag.For)
                .Generate(new LogGenerator(), Tag.While);
            Console.WriteLine(a);
            Console.WriteLine(a.Complexity);
        }
    }
}