using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using theta_bot.Classes;
using theta_bot.Extentions;

namespace theta_bot.NewGenerators.ConstGenerators
{
    public class ConstForGenerator : INewGenerator
    {
        private static readonly string[] Templates =
        {
            "for (var {0}={1}; {0}<{2}; {0}++)\n",
            "for (var {0}={1}; {0}<{2}; {0}+={3})\n",
            "for (var {0}={1}; {0}<{2}; {0}={0}+{3})\n",
            "for (var {0}={2}; {0}>{1}; {0}--)\n",
            "for (var {0}={2}; {0}>{1}; {0}-={3})\n",
            "for (var {0}={2}; {0}>{1}; {0}={0}-{3})\n",
        };

        public Exercise Generate(Exercise exercise, params Tag[] desiredTags)
        {
            var variable = exercise.NextVar(true);

            var code = new StringBuilder(exercise.Code.ToString());
            var vars = new List<Variable>(exercise.Vars) {variable};
            var tags = new List<Tag>(exercise.Tags) {Tag.For};

            var start = new Random().Next(2);
            var end = new Random().Next(1, 5) * 1000;
            var step = new Random().Next(1, 5);
            var cycleCode = string.Format(Templates.Random(), variable.Label, start, end, step);

            code.Indent(4);
            code.Insert(0, cycleCode);
            code.Insert(cycleCode.Length, "{\n");
            code.Append("}\n");

            return new Exercise(exercise, null, vars, null, code, tags);
        }
    }

    [TestFixture]
    public class ConstForShould
    {
        [Test]
        public void Test()
        {
            Console.WriteLine(
                new Exercise()
                    .Generate(new ConstGenerator(), Tag.Code)
                    .Generate(new ConstGenerator(), Tag.For)
                    .ToString());
        }
    }
}