using FluentAssertions;
using NUnit.Framework;
using Theta_Bot.Logic;

namespace Theta_Bot_Test
{
    [TestFixture]
    public class TemplateGeneratorTests
    {
        private static void Match(string template, string regex)
        {
            TemplateGenerator
                .Generate(template)
                .Should()
                .MatchRegex(regex);
        }

        [Test]
        public void SingleLoop_IncreaseToN()
        {
            Match(
@"for (var $var$=0; $var$<n; $var$$inc$)
    c++;

var: one of [i k j index]
inc: one of [++ +=1 +=2 +=3]",

@"for \(var (i|k|j|index)=0; \1<n; \1(\+\+|\+=1|\+=2|\+=3)\)
    c\+\+;");
        }

        [Test]
        public void SingleLoop_IncreaseToConstTimesN()
        {
            Match(
@"for (var i=0; i<$mult$*n; i$inc$)
    c++;

mult: one of [2 10 100]
inc: one of [++ +=1 +=2 +=3]",

@"for \(var i=0; i<(2|10|100)\*n; i(\+\+|\+=1|\+=2|\+=3)\)
    c\+\+;");
        }

        [Test]
        public void SingleLoop_DecreaseFromConstTimesN()
        {
            Match(
@"for (var i=$mult$*n; i>=0; i$dec$)
    c++;

mult: one of [2 10 100]
dec: one of [-- -=1 -=2 -=3]",

@"for \(var i=(2|10|100)\*n; i>=0; i(\-\-|\-=1|\-=2|\-=3)\)
    c\+\+;");
        }

        [Test]
        public void SingleLoop_IncreaseToNSquared()
        {
            Match(
@"for (var i=0; i<n*n; i$inc$)
    c++;

inc: one of [++ +=1 +=2 +=3]",

@"for \(var i=0; i<n\*n; i(\+\+|\+=1|\+=2|\+=3)\)
    c\+\+;");
        }

        [Test]
        public void SingleLoop_MultiplyToConstTimesN()
        {
            Match(
@"for (var i=1; i<$mult$n; i$inc$)
    c++;

mult: one of [ 2* 10*]
inc: one of [*=2 *=10 <<1 +=i]",

@"for \(var i=1; i<(2\*|10\*)?n; i(\*=2|\*=10|<<1|\+=i)\)
    c\+\+;");
        }

        [Test]
        public void SingleLoop_MultiplyToNSquared()
        {
            Match(
@"for (var i=1; i<n*n; i$inc$)
    c++;

inc: one of [*=2 *=10 <<1 +=i]",

@"for \(var i=1; i<n\*n; i(\*=2|\*=10|<<1|\+=i)\)
    c\+\+;");
        }

        [Test]
        public void SingleLoop_DivideFromNSquared()
        {
            Match(
@"for (var i=n*n; i>1; i$dec$)
    c++;

dec: one of [/=2 /=10 >>1 -=i]",

@"for \(var i=n\*n; i>1; i(/=2|/=10|>>1|\-=i)\)
    c\+\+;");
        }

        [Test]
        public void DoubleLoop()
        {
            Match(
@"for (var i=1; i<$mult$n; i$inc$)
    for (var j=1; j<$mult$n; j$inc$)
        c++;

mult: one of [ 2* 10*]
inc: one of [*=2 *=10 <<1 +=i]",

@"for \(var i=1; i<(2\*|10\*)?n; i(\*=2|\*=10|<<1|\+=i)\)
    for \(var j=1; j<(2\*|10\*)?n; j(\*=2|\*=10|<<1|\+=i)\)
        c\+\+;");
        }
    }
}
