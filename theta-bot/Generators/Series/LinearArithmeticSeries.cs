using System;
using System.Text;
using FluentAssertions;
using NUnit.Framework;
using theta_bot.Classes;
using theta_bot.Generators;

namespace theta_bot
{
    public class LinearArithmeticSeries : IGenerator
    {   
        public void ChangeCode(StringBuilder code, Func<Variable> getNextVar, Random random) => 
            Series.ChangeCode(code, getNextVar, random, new LinearForLoop(), 
                Series.ArithmeticTemplates);

        public bool TryGetComplexity(Complexity oldComplexity, out Complexity newComplexity)
        {
            newComplexity = oldComplexity < Complexity.N2
                ? Complexity.N2
                : oldComplexity;
            return true;
        }
    }
    
    [TestFixture]
    public class LinearArithmeticSeries_Should
    {
        [Test]
        public void Generate()
        {
            var exercise = new Exercise()
                .Generate(new LinearArithmeticSeries(), new Random());
            Console.WriteLine(exercise.ToString);
            exercise.Complexity.Should().Be(Complexity.N2);
        }
    }
}