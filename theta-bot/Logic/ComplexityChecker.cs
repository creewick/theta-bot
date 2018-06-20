﻿using System;
using System.Linq;
 using FluentAssertions;
 using NUnit.Framework;
using theta_bot.Classes;
using theta_bot.Classes.Enums;

namespace theta_bot.Logic
{
    public class ComplexityChecker
    {
        private static Complexity[,] Answer = new Complexity[12,3]
        {
            {Complexity.Log, Complexity.N, Complexity.Const},     // J < N; J += N; (and default)
            {Complexity.N, Complexity.NLog, Complexity.N},       //             I;
            {Complexity.NLog, Complexity.N2, Complexity.N},      //             1;
            
            {Complexity.Log, Complexity.N, Complexity.Const},     //        J *= N;
            {Complexity.Log2, Complexity.NLog, Complexity.Log},  //             I;
            {Complexity.Log2, Complexity.NLog, Complexity.Log},  //             1;

            {Complexity.Log, Complexity.N, Complexity.Const},     // J < I; J += N;
            {Complexity.Log, Complexity.N, Complexity.Const},     //             I;
            {Complexity.N, Complexity.N2, Complexity.Const},      //             1;
            
            {Complexity.Log, Complexity.N, Complexity.Const},     //        J *= N;
            {Complexity.Log, Complexity.N, Complexity.Const},     //             I;
            {Complexity.Log2, Complexity.NLog, Complexity.Const},//             1;
        };

        public static Complexity Check(Exercise exercise)
        {
            if (exercise is DoubleLoopExercise d)
                return Check(d);
            if (exercise is SingleLoopExercise s)
                return Check(s);
            throw new NotImplementedException();
        }
       
        public static Complexity Check(DoubleLoopExercise exercise)
        {
            var column = GetColumn(exercise.OuterLoop);
            var row = GetRow(exercise.InnerLoop);
            return Answer[row, column];
        }
        
        public static Complexity Check(SingleLoopExercise exercise)
        {
            var column = GetColumn(exercise.Loop);
            return Answer[0, column];
        }

        private static int GetRow(Loop inner)
        {
            var row = 0;
            if (inner.Bound == VarType.Const)
                return 0;
            if (inner.Bound == VarType.Prev)
                row += 6;
            if (inner.Operation == OpType.Multiply)
                row += 3;
            if (inner.Step == VarType.Prev)
                row += 1;
            if (inner.Step == VarType.Const)
                row += 2;
            return row;
        }

        private static int GetColumn(Loop outer)
        {
            if (outer.Bound == VarType.N &&
                outer.Step == VarType.Const)
                return outer.Operation == OpType.Multiply
                    ? 0
                    : 1;
            return 2;
        }
    }

    [TestFixture]
    public class DoubleCode_Should
    {
        [Test]
        public void TestAllCases(
            [Values(VarType.Const, VarType.N)] VarType outerBound,
            [Values(OpType.Increase, OpType.Multiply)]OpType outerOp,
            [Values(VarType.Const, VarType.N)] VarType outerStep,
            [Values(VarType.Const, VarType.N)] VarType innerBound,
            [Values(OpType.Increase, OpType.Multiply)]OpType innerOp
            )
        {
            var varTypes = Enum.GetValues(typeof(VarType)).Cast<VarType>();
            var random = new Random(1224);
            foreach (var innerStep in varTypes)
            {
                var outer = new Loop(outerBound, outerOp, outerStep);
                var inner = new Loop(innerBound, innerOp, innerStep);
                var exercise = new DoubleLoopExercise(outer, LoopType.For, inner, LoopType.For);
                exercise.GetComplexity()
                    .Should()
                    .Be(ComplexityChecker.Check(exercise), exercise.GetCode(random));
            }
        }
    }
    
    [TestFixture]
    public class SingleLoop_Should
    {
        [Test]
        public void TestAllCases(
            [Values(VarType.Const, VarType.N)] VarType outerBound,
            [Values(OpType.Increase, OpType.Multiply)]OpType outerOp,
            [Values(VarType.Const, VarType.N)] VarType outerStep
        )
        {
            var varTypes = Enum.GetValues(typeof(VarType)).Cast<VarType>();
            var random = new Random(1224);
            foreach (var innerStep in varTypes)
            {
                var outer = new Loop(outerBound, outerOp, outerStep);
                var exercise = new SingleLoopExercise(outer, LoopType.For);
                exercise.GetComplexity()
                    .Should()
                    .Be(ComplexityChecker.Check(exercise), exercise.GetCode(random));
            }
        }
    }
}