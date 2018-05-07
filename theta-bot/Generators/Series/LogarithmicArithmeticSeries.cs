using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using NUnit.Framework;
using theta_bot.Classes;
using theta_bot.Generators;

namespace theta_bot
{
    public class LogarithmicArithmeticSeries : IGenerator
    {
        public void ChangeCode(StringBuilder code, Func<Variable> getNextVar, Random random) => 
            Series.ChangeCode(code, getNextVar, random, new LogarithmicForLoop(), Series.ArithmeticTemplates);

        public bool TryGetComplexity(Complexity oldComplexity, out Complexity newComplexity)
        {
            newComplexity = oldComplexity < Complexity.NLogN
                ? Complexity.NLogN
                : oldComplexity;
            return true;
        }
    }
    
    [TestFixture]
    public class LogarithmicArithmeticSeries_Should
    {
        [Test]
        public void Generate()
        {
            var exercise = new Exercise()
                .Generate(new LogarithmicArithmeticSeries(), new Random());
            Console.WriteLine(exercise.ToString);
            exercise.Complexity.Should().Be(Complexity.NLogN);
        }
    }
}