using System;
using System.Collections.Generic;
using System.Linq;

namespace theta_bot.Generators
{
    public class SimpleLoopGenerator : Generator
    {
        private readonly Dictionary<Complexity, Complexity> complexity = 
            new Dictionary<Complexity, Complexity>
        {
            {Complexity.Constant, Complexity.Linear},
            {Complexity.Linear, Complexity.Quadratic},
            {Complexity.Quadratic, Complexity.Cubic},
            {Complexity.Logarithmic, Complexity.Polynomial}
        };
        
        private readonly string[] templates =
        {
            "for (var {0} = 0; {0} < {2}; {0}++)\n",
            "for (var {0} = 0; {0} < {2}; {0} += {3})\n"
        };
        
        public Exercise Generate(Exercise exercise, Random random)
        {
            var variable = exercise.Vars
                .Where(v => !v.IsBounded)
                .Shuffle(random)
                .First();
            
            var startValue = random.Next(2);
            var endValue = random.Next(1, 5) * 1000;
            var stepValue = random.Next(1, 5);
            var template = templates[random.Next(templates.Length)];
            
            var code = string.Format(template, variable.Label, startValue, endValue, stepValue);
            exercise.Code.ShiftLines(4);
            exercise.Code.Insert(0, code);
            exercise.Code.Insert(code.Length, "{\n");
            exercise.Code.Append("}");
            exercise.Complexity = GetComplexity(exercise.Complexity);
            
            variable.IsBounded = true;
            return exercise;
        }

        public Complexity GetComplexity(Complexity previousComplexity) => complexity[previousComplexity];
    }
}