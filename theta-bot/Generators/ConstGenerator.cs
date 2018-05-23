using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using theta_bot.Classes;
using theta_bot.Extentions;

namespace theta_bot.Generators
{
    public class ConstGenerator : INewGenerator
    {   
        private static readonly string[] CodeTemplates =
        {
            "{0}+=1;",
            "{0}++;"
        };
        
        private static readonly string[] ForTemplates =
        {
            "for (var {0}={1}; {0}<{2}; {0}++)\n{{\n",
            "for (var {0}={1}; {0}<{2}; {0}+={3})\n{{\n",
            "for (var {0}={1}; {0}<{2}; {0}={0}+{3})\n{{\n",
            "for (var {0}={2}; {0}>{1}; {0}--)\n{{\n",
            "for (var {0}={2}; {0}>{1}; {0}-={3})\n{{\n",
            "for (var {0}={2}; {0}>{1}; {0}={0}-{3})\n{{\n",
        };

        private static readonly string[] WhileTemplates =
        {
            "var {0} = {1};\nwhile ({0} < {2})\n{{\n    {0}++;\n",
            "var {0} = {1};\nwhile ({0} < {2})\n{{\n    {0}+={3};\n",
            "var {0} = {1};\nwhile ({0} < {2})\n{{\n    {0} = {0} + {3};\n",
        };
        
        public static readonly Dictionary<Tag, string[]> Templates = 
            new Dictionary<Tag, string[]>
        {
            {Tag.Code, CodeTemplates},
            {Tag.For, ForTemplates}
        };
        
        public Exercise Generate(Exercise exercise, params Tag[] tags)
        {
            exercise = exercise.Copy();
            var mainTag = MainTag(tags);
            
            var variable = (mainTag == Tag.Code)
                ? new Variable("count")
                : exercise.AddNewVar(true);

            var start = new Random().Next(2);
            var end = new Random().Next(1, 5) * 1000;
            var step = new Random().Next(1, 5);
            var template = Templates[mainTag].Random();

            var code = string.Format(template, variable, start, end, step);

            if (mainTag == Tag.Code)
                exercise.Code.AppendLine(code);
            else
            {
                exercise.Code.Indent(4);
                exercise.Code.Insert(0, code);
                exercise.Code.Append("}\n");
            }

            exercise.Tags.Add(mainTag);
            return exercise;
        }

        private static Tag MainTag(params Tag[] tags)
        {
            if (tags.Contains(Tag.Code))
                return Tag.Code;
            if (tags.Contains(Tag.For))
                return Tag.For;
            if (tags.Contains(Tag.While))
                return Tag.While;
            throw new ArgumentException("Generator called without needed tag");
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
                    .Generate(new ConstGenerator(), Tag.For));
        }
    }
}