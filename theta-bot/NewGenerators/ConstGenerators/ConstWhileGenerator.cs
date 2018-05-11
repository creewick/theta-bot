using System;
using System.Collections.Generic;
using System.Text;
using theta_bot.Classes;
using theta_bot.Extentions;

namespace theta_bot.NewGenerators.ConstGenerators
{
    public class ConstWhileGenerator : INewGenerator
    {
        private static readonly string[] Templates =
        {
            "var {0} = {1};\nwhile({0} < {2})\n{{\n    {0}++;\n",
            "var {0} = {1};\nwhile({0} < {2})\n{{\n    {0}+={3};\n",
            "var {0} = {1};\nwhile({0} < {2})\n{{\n    {0} = {0} + {3};\n",
        };

        public Exercise Generate(Exercise exercise, params Tag[] desiredTags)
        {
            var variable = exercise.NextVar(true);
            
            var code = new StringBuilder(exercise.Code.ToString());
            var vars = new List<Variable>(exercise.Vars) {variable};
            var tags = new List<Tag>(exercise.Tags) {Tag.While};
            
            var start = new Random().Next(2);
            var end = new Random().Next(1, 5) * 1000;
            var step = new Random().Next(1, 5);
            var cycleCode = string.Format(Templates.Random(), variable.Label, start, end, step);

            code.Indent(4);
            code.Insert(0, cycleCode);
            code.Append("}\n");
            
            return new Exercise(exercise, null, vars, null, code, tags);
        }
    }
}