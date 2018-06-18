﻿using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using theta_bot.Classes;
using theta_bot.Classes.Enums;
using theta_bot.Extentions;

namespace theta_bot.Logic
{
    public class SingleLoopExercise : Exercise
    {
        public readonly LoopType LoopType;
        public readonly Loop Loop;

        public SingleLoopExercise(Loop loop, LoopType loopType)
        {
            if (loop.Bound == VarType.Prev || loop.Step == VarType.Prev) 
                throw new ArgumentException("Outer cycle can't depend on I");
            Loop = loop;
            LoopType = loopType;
        }
        
        public override string GetCode(Random random)
        {
            var loop = CodeGenerator.GetLoopCode(Loop, LoopType, random);
            return $"var count=0;\n{loop}    count++;\n}}";
        }

        public override int RunCode(double n)
        {
            var b = Loop.Bound;
            var op = Loop.Operation;
            var s = Loop.Step;
            
            var count = 0;
            for (var i = 1.0; Bound(i, 0, n, b); i = Step(i, 0, n, op, s))
                count++;
            return count;
        }
        
        public override IEnumerable<Complexity> GenerateOptions(Random random, int count)
        {
            var complexity = GetComplexity();
            return Complexity.All
                .Where(c => !c.Equals(complexity))
                .Shuffle()
                .Take(3)
                .Concat(GetComplexity());
        }
    }

    [TestFixture]
    public class SingleLoopTest
    {
        [Test]
        public void Test1()
        {
            var random = new Random();
            var loop = new Loop(
                new[] {VarType.Const, VarType.N}.Random(random),
                new[] {OpType.Increase, OpType.Multiply}.Random(random),
                new[] {VarType.Const, VarType.N}.Random(random));
            Console.WriteLine(new SingleLoopExercise(loop, LoopType.For).GetComplexity());
        }
    }
}