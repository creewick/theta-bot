using System;
using System.Linq;
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
       
        public static Complexity Check(DoubleLoopExercise exercise)
        {
            var column = GetColumn(exercise.OuterLoop);
            var row = GetRow(exercise.InnerLoop);
            return Answer[row, column];
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
        public void TestAllCases()
        {
            var varTypes = Enum.GetValues(typeof(VarType)).Cast<VarType>();
            var opTypes = Enum.GetValues(typeof(OpType)).Cast<OpType>();
            foreach (var outerBound in new[] {VarType.Const, VarType.N})
                foreach (var outerOp in opTypes)
                    foreach (var outerStep in new[] {VarType.Const, VarType.N})
                        foreach (var innerBound in varTypes)
                            foreach (var innerOp in opTypes)
                                foreach (var innerStep in varTypes)
                                {
                                    var outer = new Loop(outerBound, outerOp, outerStep);
                                    var inner = new Loop(innerBound, innerOp, innerStep);
                                    var exercise = new DoubleLoopExercise(outer, LoopType.For, inner, LoopType.For);
                                    Console.Write(ComplexityChecker.Check(exercise).Equals(exercise.GetComplexity())
                                        ? '+' : '-');
                                    
                                }
        }
    }
}