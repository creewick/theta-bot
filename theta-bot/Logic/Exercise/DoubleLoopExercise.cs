using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using theta_bot.Classes;
using theta_bot.Classes.Enums;

namespace theta_bot.Logic
{
    public class DoubleLoopExercise : Exercise
    {
        public readonly Loop OuterLoop;
        public readonly LoopType OuterLoopType;
        public readonly Loop InnerLoop;
        public readonly LoopType InnerLoopType;

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
            code.Insert(0, CodeGenerator.GetLoopCode(InnerLoop, InnerLoopType, random));
            code.Append("}\n");
            code.Indent(4);
            code.Insert(0, CodeGenerator.GetLoopCode(OuterLoop, OuterLoopType, random));
            code.Append("}\n");
            code.Insert(0, "var count=0;");
            return code.ToString();
        }

        public override int RunCode(int n)
        {
            var b2 = OuterLoop.Bound;
            var op2 = OuterLoop.Operation;
            var s2 = OuterLoop.Step;
            var b1 = InnerLoop.Bound;
            var op1 = InnerLoop.Operation;
            var s1 = InnerLoop.Step;
            
            var count = 0;
            for (var i = 1; Bound(i, 0, n, b2); i = Step(i, 0, n, op2, s2))
                for (var j = 1; Bound(j, i, n, b1); j = Step(j, i, n, op1, s1))
                    count++;
            return count;
        }
        
        public override IEnumerable<Complexity> GenerateOptions(Random random, int count)
        {
            throw new NotImplementedException();
        }
    }

    [TestFixture]
    public class DoubleLoop_Should
    {
        [Test]
        public void N2()
        {
            var innerLoop = new Loop(VarType.N, OpType.Increase, VarType.Const);
            var outerLoop = new Loop(VarType.N, OpType.Increase, VarType.Const);
            var exercise = new DoubleLoopExercise(outerLoop, LoopType.For, innerLoop, LoopType.For);
            Assert.AreEqual(new Complexity(2, 0), exercise.GetComplexity());
        }
        
        [Test]
        public void NLogN()
        {
            var innerLoop = new Loop(VarType.N, OpType.Increase, VarType.Const);
            var outerLoop = new Loop(VarType.N, OpType.Multiply, VarType.Const);
            var exercise = new DoubleLoopExercise(outerLoop, LoopType.For, innerLoop, LoopType.For);
            Assert.AreEqual(new Complexity(1, 1), exercise.GetComplexity());
        }
        
        [Test]
        public void Log2N()
        {
            var innerLoop = new Loop(VarType.N, OpType.Multiply, VarType.Const);
            var outerLoop = new Loop(VarType.N, OpType.Multiply, VarType.Const);
            var exercise = new DoubleLoopExercise(outerLoop, LoopType.For, innerLoop, LoopType.For);
            Assert.AreEqual(new Complexity(0, 2), exercise.GetComplexity());
        }
        
        [Test]
        public void LogDependOnLinearByStep_N()
        {
            var innerLoop = new Loop(VarType.N, OpType.Multiply, VarType.Prev);
            var outerLoop = new Loop(VarType.N, OpType.Increase, VarType.Const);
            var exercise = new DoubleLoopExercise(outerLoop, LoopType.For, innerLoop, LoopType.For);
            Assert.AreEqual(new Complexity(1, 0), exercise.GetComplexity());
        }
        
        [Test]
        public void GeometricalProgression_N()
        {
            var innerLoop = new Loop(VarType.N, OpType.Increase, VarType.Prev);
            var outerLoop = new Loop(VarType.N, OpType.Multiply, VarType.Const);
            var exercise = new DoubleLoopExercise(outerLoop, LoopType.For, innerLoop, LoopType.For);
            Assert.AreEqual(new Complexity(1, 0), exercise.GetComplexity());
        }
        
        [Test]
        public void AriphmeticalProgression_N2()
        {
            var innerLoop = new Loop(VarType.Prev, OpType.Increase, VarType.Const);
            var outerLoop = new Loop(VarType.N, OpType.Increase, VarType.Const);
            var exercise = new DoubleLoopExercise(outerLoop, LoopType.For, innerLoop, LoopType.For);
            Assert.AreEqual(new Complexity(2, 0), exercise.GetComplexity());
        }
        
        [Test]
        public void HarmonicalProgression_NLogN()
        {
            var innerLoop = new Loop(VarType.N, OpType.Increase, VarType.Prev);
            var outerLoop = new Loop(VarType.N, OpType.Increase, VarType.Const);
            var exercise = new DoubleLoopExercise(outerLoop, LoopType.For, innerLoop, LoopType.For);
            Assert.AreEqual(new Complexity(1, 1), exercise.GetComplexity());
        }
    }
}