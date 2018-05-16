//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using NUnit.Framework;
//using theta_bot.Classes;
//using theta_bot.Extentions;
//using theta_bot.NewGenerators.ConstGenerators;
//
//namespace theta_bot.NewGenerators.LogGenerators
//{
//    public class LogForGenerator : INewGenerator
//    {
//        private static readonly string[] Templates =
//        {
//            "for (var {0}={1}; {0}<{2}; {0}*={3})\n",
//            "for (var {0}={1}; {0}<{2}*{2}; {0}*={3})\n",
//            "for (var {0}={1}; {0}<{2}; {0}={0}*{3})\n",
//            "for (var {0}={2}; {0}>{1}; {0}/={3})\n",
//            "for (var {0}={2}*{2}; {0}>{1}; {0}/={3})\n",
//            "for (var {0}={2}; {0}>{1}; {0}={0}/{3})\n"
//        };
//
//        public Exercise Generate(Exercise exercise, params Tag[] tags)
//        {
//            bool depend = desiredTags.Contains(Tag.Depend);
//            
//            var code = new StringBuilder(exercise.Code.ToString());
//            var vars = new List<Variable>(exercise.Vars);
//            var tags = new List<Tag>(exercise.Tags);
//            if (depend) tags.Add(Tag.For);
//            
//            var cycleVar = depend 
//                ? exercise.MainVar 
//                : exercise.NextVar(true);
//            var start = new Random().Next(1, 3);
//            var end = depend
//                ? exercise.NextVar(true)
//                : exercise.MainVar;
//            var step = new Random().Next(2, 5);
//
//            var cycleCode = string.Format(Templates.Random(), cycleVar.Label, start, end.Label, step);
//
//            var complexity = new Complexity(
//                exercise.Complexity.N, 
//                exercise.Complexity.LogN + 1);
//            
//            code.Indent(4);
//            code.Insert(0, cycleCode);
//            code.Insert(cycleCode.Length, "{\n");
//            code.Append("}\n");
//
//            return new Exercise(exercise, complexity, vars, end, code, tags);
//        }
//    }
//
//    [TestFixture]
//    public class LogForShould
//    {
//        [Test]
//        public void Test()
//        {
//            Console.WriteLine(
//                new Exercise()
//                    .Generate(new ConstGenerator(), Tag.Code)
//                    .Generate(new LogGenerator(), Tag.For)
//                    .Generate(new LogGenerator(), Tag.For, Tag.Depend)
//                    .ToString());
//        }
//    }
//}