using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using theta_bot.Classes;
using theta_bot.Extentions;
using theta_bot.NewGenerators.ConstGenerators;

namespace theta_bot.NewGenerators.LogGenerators
{
    public class LogForGenerator : INewGenerator
    {
        private static readonly string[] Templates =
        {
            "for (var {0}={1}; {0}<{2}; {0}*={3})\n",
            "for (var {0}={1}; {0}<{2}*{2}; {0}*={3})\n",
            "for (var {0}={1}; {0}<{2}; {0}={0}*{3})\n",
            "for (var {0}={2}; {0}>{1}; {0}/={3})\n",
            "for (var {0}={2}*{2}; {0}>{1}; {0}/={3})\n",
            "for (var {0}={2}; {0}>{1}; {0}={0}/{3})\n"
        };

        public Exercise Generate(Exercise exercise, params Tag[] desiredTags)
        {
            var variable = exercise.NextVar(true);
            
            var code = new StringBuilder(exercise.Code.ToString());
            var vars = new List<Variable>(exercise.Vars) {variable};
            var tags = new List<Tag>(exercise.Tags) {Tag.For};
            
            var start = new Random().Next(1, 3);
            var end = exercise.MainVar;
            if (desiredTags.Contains(Tag.Depend))
            {
                end = exercise.NextVar();
                vars.Add(end);
                tags.Add(Tag.Depend);
            }
            
            var step = new Random().Next(2, 5);
            var template = Templates.Random();
            var cycleCode = string.Format(template, variable.Label, start, end.Label, step);

            var complexity = exercise.Complexity;
            
            code.Indent(4);
            code.Insert(0, cycleCode);
            code.Insert(cycleCode.Length, "{\n");
            code.Append("}\n");

            return new Exercise(exercise, complexity, vars, end, code, tags);
        }
    }

    [TestFixture]
    public class LogForShould
    {
        [Test]
        public void Test()
        {
            Console.WriteLine(
                new Exercise()
                    .Generate(new ConstGenerator(), Tag.Code)
                    .Generate(new LogGenerator(), Tag.For)
                    .Generate(new LogGenerator(), Tag.For, Tag.Depend)
                    .ToString());
        }
    }
}