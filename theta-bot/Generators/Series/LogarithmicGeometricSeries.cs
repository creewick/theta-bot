using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace theta_bot
{
    public class LogarithmicGeomrtricSeries : IGenerator
    {
        public void ChangeCode(StringBuilder code, Func<Variable> getNextVar, Random random) => 
            Series.ChangeCode(code, getNextVar, random, new LogarithmicForLoop(), Series.GeometricTemplates);

        public bool TryGetComplexity(Complexity oldComplexity, out Complexity newComplexity)
        {
            newComplexity = oldComplexity < Complexity.Log2N
                ? Complexity.Log2N
                : oldComplexity;
            return true;
        }
    }
    
    [TestFixture]
    public class LogarithmicGeometricSeries_Should
    {
        [Test]
        public void Generate()
        {
            var exercise = new Exercise()
                .Generate(new LogarithmicGeomrtricSeries(), new Random());
            Console.WriteLine(exercise.Message);
            exercise.Complexity.Should().Be(Complexity.Log2N);
        }
    }
}