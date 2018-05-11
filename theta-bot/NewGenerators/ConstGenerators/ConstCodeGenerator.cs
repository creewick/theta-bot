using System.Collections.Generic;
using System.Text;
using theta_bot.Classes;
using theta_bot.Extentions;

namespace theta_bot.NewGenerators.ConstGenerators
{
    public class ConstCodeGenerator : INewGenerator
    {
        private static readonly string[] Templates =
        {
            "{0}+=1;",
            "{0}++;"
        };
        
        public Exercise Generate(Exercise previous, params Tag[] desiredTags)
        {
            var variable = new Variable("count");
            
            var code = new StringBuilder(previous.Code.ToString());
            var tags = new List<Tag>(previous.Tags) {Tag.Code};
            var vars = new List<Variable>(previous.Vars) {variable};

            code.AppendLine(string.Format(Templates.Random(), variable.Label));

            return new Exercise(previous, null, vars, null, code, tags);
        }
    }
}