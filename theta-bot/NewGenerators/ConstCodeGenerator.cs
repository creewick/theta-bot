using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using theta_bot.Classes;

namespace theta_bot.NewGenerators
{
    public static class ConstCodeGenerator
    {
        private static readonly string[] templates =
        {
            "count+=1;",
            "count++;"
        };
        
        public static Exercise Generate(Exercise previous)
        {
            var code = new StringBuilder(previous.ToString());
            var newTags = new List<Tag>(previous.Tags).Append(Tag.Code).ToList();
            
            var template = templates[new Random().Next(templates.Length)];
            code.AppendLine(template);

            return new Exercise(previous.Complexity, previous.Vars, code, newTags);
        }
    }
}