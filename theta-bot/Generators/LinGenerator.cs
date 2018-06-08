using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using theta_bot.Classes;
using theta_bot.Extentions;

namespace theta_bot.Generators
{
    public class LinGenerator : Generator
    {   
        private static readonly string[] ForTemplates =
        {
            "for (var {0}={1}; {0}<{2}; {0}+={3})\n{{\n",
            "for (var {0}={1}; {0}<{2}; {0}={0}+{3})\n{{\n",
            "for (var {0}={2}; {0}>{1}; {0}-={3})\n{{\n",
            "for (var {0}={2}; {0}>{1}; {0}={0}-{3})\n{{\n",
        };

        private static readonly string[] WhileTemplates =
        {
            "var {0} = {1};\nwhile ({0} < {2})\n{{\n    {0} += {3};\n",
            "var {0} = {1};\nwhile ({0} < {2})\n{{\n    {0} = {0} + {3};\n",
            "var {0} = {2};\nwhile ({0} < {1})\n{{\n    {0} -= {3};\n",
            "var {0} = {2};\nwhile ({0} > {1})\n{{\n    {0} = {0} - {3};\n",
        };

        private static readonly Dictionary<LoopType, string[]> Templates = 
            new Dictionary<LoopType, string[]>
            {
                {LoopType.For, ForTemplates},
                {LoopType.While, WhileTemplates}
            };
        
        public override IExercise Generate(IExercise exercise, Random random, params LoopType[] loopTypes)
        {
            var complexity = GetComplexity(exercise, loopTypes);
            if (complexity == null) return exercise;

            var codeType = CodeType(loopTypes);
            var newVar = new Variable(true);
            
            var start = new Variable(random.Next(1, 3));
            var end = exercise.MainVar;
            var step = new Variable(random.Next(2, 5));
            var template = Templates[codeType].Random();

            var newCode = string.Format(template, newVar, start, end, step);
            var newTags = new List<LoopType> {codeType};
            var newVars = new List<Variable> {newVar, start, end, step};

            if (loopTypes.Contains(LoopType.DependFromValue))
            {
                exercise = exercise.ReplaceVar(exercise.Vars[2], newVar);
                newTags.Add(LoopType.DependFromValue);
            }

            if (loopTypes.Contains(LoopType.DependFromStep))
            {
                exercise = exercise.ReplaceVar(exercise.Vars[3], newVar);
                newTags.Add(LoopType.DependFromStep);
            }
            
            return new IExercise(newVars, newCode, newTags, (Complexity)complexity, exercise);
        }

        private static Complexity? GetComplexity(IExercise previous, LoopType[] loopTypes)
        {
            if (!Depend(loopTypes))
                return Complexity.Linear;
            if (previous.Complexity == Complexity.Const)
                return Complexity.Linear;
            if (previous.Complexity == Complexity.Log)
                return loopTypes.Contains(LoopType.DependFromValue)
                    ? new Complexity(1, 1)
                    : Complexity.Linear;
            if (previous.Complexity == Complexity.Linear)
                return loopTypes.Contains(LoopType.DependFromValue)
                    ? new Complexity(2, 0)
                    : new Complexity(1, 1);
            return null;
        }
    }
    
    [TestFixture]
    public class LinForShould
    {
        [Test]
        public void Test()
        {
            var a = new IExercise()
                .Generate(new ConstGenerator(), LoopType.Code)
                .Generate(new LinGenerator(), LoopType.For)
                .Generate(new LinGenerator(), LoopType.While)
                .Build();
            Console.WriteLine(a);
            Console.WriteLine(a.Complexity);
        }
        
        [Test]
        public void Test2()
        {
            var a = new IExercise()
                .Generate(new ConstGenerator(), LoopType.Code)
                .Generate(new LinGenerator(), LoopType.For)
                .Generate(new LinGenerator(), LoopType.For, LoopType.DependFromValue)
                .Build();
            Console.WriteLine(a);
            Console.WriteLine(a.Complexity);
        }
        
        [Test]
        public void Test3()
        {
            var a = new IExercise()
                .Generate(new ConstGenerator(), LoopType.Code)
                .Generate(new LinGenerator(), LoopType.For)
                .Generate(new LinGenerator(), LoopType.For, LoopType.DependFromStep)
                .Build();
            Console.WriteLine(a);
            Console.WriteLine(a.Complexity);
        }
    }
}