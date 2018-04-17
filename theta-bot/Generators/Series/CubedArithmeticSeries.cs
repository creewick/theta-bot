using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace theta_bot.Series
{
    public class CubedArithmeticSeries : IGenerator
    {
        private readonly Dictionary<Complexity, Complexity> complexities = 
            new Dictionary<Complexity, Complexity>
            {{Complexity.Constant, Complexity.Cubic}};
                
        private readonly string[] outerLoop =
        {
            "for (var {0}=0; {0}<n; {0}++)\n",
            "for (var {0}=0; {0}<n; {0}+=1)\n",
            "for (var {0}=0; {0}<n; {0}+={1})\n",
            "for (var {0}=n; {0}>0; {0}--)\n",
            "for (var {0}=n; {0}>0; {0}-=1)\n",
            "for (var {0}=n; {0}>0; {0}-={1})\n",
        };
        
        private readonly string[] innerLoop =
        {
            "for (var {0}=0; {0}<{1}*n; {0}++)\n",
            "for (var {0}=0; {0}<{1}*n; {0}+=1)\n",
            "for (var {0}={1}*n; {0}>0; {0}--)\n",
            "for (var {0}={1}*n; {0}>0; {0}-=1)\n",
        };

        public void ChangeCode(StringBuilder code, Func<Variable> getNextVar, Random random)
        {
            var innerVariable = getNextVar();
            innerVariable.IsBounded = true;
            var outerVariable = getNextVar();
            outerVariable.IsBounded = true;
            var stepValue = random.Next(2, 5);
            
            var innerTemplate = innerLoop[random.Next(innerLoop.Length)];
            var innerLoopCode = string.Format(innerTemplate, innerVariable.Label, outerVariable.Label);
                    
            code.ShiftLines(4);
            code.Insert(0, innerLoopCode);
            code.Insert(innerLoopCode.Length, "{\n");
            code.Append("}\n");

            var outerTemplate = outerLoop[random.Next(outerLoop.Length)];
            var outerLoopCode = string.Format(outerTemplate, outerVariable.Label, stepValue);
            
            code.ShiftLines(4);
            code.Insert(0, outerLoopCode);
            code.Insert(outerLoopCode.Length, "{\n");
            code.Append("}\n");
        }

        public bool TryGetComplexity(Complexity oldComplexity, out Complexity newComplexity)
        {
            newComplexity = complexities.ContainsKey(oldComplexity)
                ? complexities[oldComplexity]
                : oldComplexity;
            return complexities.ContainsKey(oldComplexity);
        }
    }
    
    [TestFixture]
    public class CubedArithmeticSeries_Should
    {
        [Test]
        public void Generate()
        {
            var exercise = new Exercise()
                .Generate(new SimpleCodeBlock(), new Random())
                .Generate(new CubedArithmeticSeries(), new Random());
            Console.WriteLine(exercise.Message);
            exercise.Complexity.Should().Be(Complexity.Cubic);
        }
    }
}