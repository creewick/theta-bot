﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using theta_bot.Classes;
using theta_bot.Classes.Enums;
using theta_bot.Extentions;

namespace theta_bot.Logic
{
    public class DoubleLoopExercise : Exercise
    {        
        public readonly LoopType OuterLoopType;
        public readonly Loop OuterLoop;
        public readonly LoopType InnerLoopType;
        public readonly Loop InnerLoop;

        public DoubleLoopExercise(Loop outerLoop, LoopType outerLoopType, 
            Loop innerLoop, LoopType innerLoopType)
        {
            if (outerLoop.Bound == VarType.Prev || outerLoop.Step == VarType.Prev) 
                throw new ArgumentException("Outer cycle can't depend on I");
            OuterLoop = outerLoop;
            OuterLoopType = outerLoopType;
            InnerLoop = innerLoop;
            InnerLoopType = innerLoopType;
        }

        public override string GetCode(Random random)
        {
            var code = new StringBuilder("count++;\n");
            code.Indent(4);
            code.Insert(0, CodeGenerator.GetLoopCode(InnerLoop, InnerLoopType, "j", random));
            code.Append("}\n");
            code.Indent(4);
            code.Insert(0, CodeGenerator.GetLoopCode(OuterLoop, OuterLoopType, "i", random));
            code.Append("}\n");
            code.Insert(0, "var count=0;\n");
            return code.ToString();
        }

        public override int RunCode(double n)
        {
            var b2 = OuterLoop.Bound;
            var op2 = OuterLoop.Operation;
            var s2 = OuterLoop.Step;
            var b1 = InnerLoop.Bound;
            var op1 = InnerLoop.Operation;
            var s1 = InnerLoop.Step;
            
            var count = 0;
            for (var i = 1.0; Bound(i, 0, n, b2); i = Step(i, 0, n, op2, s2))
                for (var j = 1.0; Bound(j, i, n, b1); j = Step(j, i, n, op1, s1))
                {
                    count++;
                }
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
    public class RunCode_Should
    {
        [Test]
        public void Test1()
        {
            var outer = new Loop(VarType.Const, OpType.Increase, VarType.Const);
            var inner = new Loop(VarType.N, OpType.Increase, VarType.Const);
        }
    }
}