using System;
using System.Collections.Generic;
using System.Linq;
using theta_bot.Classes;
using theta_bot.Classes.Enums;
using theta_bot.Extentions;
using theta_bot.Generators;

namespace theta_bot.Levels
{
    public class Level0 : ILevel
    {
        public bool IsFinished(IEnumerable<bool?> stats, long chatId) =>
            stats.Count() >= 5 &&
            stats.Reverse()
                .Take(5)
                .All(stat => stat != null && (bool)stat);

        public IExercise Generate(Random random) =>
            new SingleLoopExercise(
                new[] {VarType.Const, VarType.N}.Random(random),
                new[] {OpType.Increase, OpType.Multiply}.Random(random),
                new[] {VarType.Const, VarType.N}.Random(random),
                LoopType.For
            );
    }
}