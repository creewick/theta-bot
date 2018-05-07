using System;
using System.Text;
using FluentAssertions;
using NUnit.Framework;
using theta_bot.Classes;
using theta_bot.Generators;

namespace theta_bot
{
    public class LinearGeometricSeries : IGenerator
    {   
        public void ChangeCode(StringBuilder code, Func<Variable> getNextVar, Random random) => 
            Series.ChangeCode(code, getNextVar, random, new LinearForLoop(), Series.GeometricTemplates);

        public bool TryGetComplexity(Complexity oldComplexity, out Complexity newComplexity)
        {
            newComplexity = oldComplexity < Complexity.N
                ? Complexity.N
                : oldComplexity;
            return true;
        }
    }
    
    [TestFixture]
    public class LinearGeometricSeries_Should
    {
        [Test]
        public void Generate()
        {
            var exercise = new Exercise()
                .Generate(new SimpleCodeBlock(), new Random())
                .Generate(new LinearForLoop(), new Random())
                .Generate(new LinearArithmeticSeries(), new Random());
            Console.WriteLine(exercise.ToString);
            exercise.Complexity.Should().Be(Complexity.N);
        }
    }
}