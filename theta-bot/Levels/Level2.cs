using System;
using System.Collections.Generic;
using System.Linq;
using theta_bot.Classes;
using theta_bot.Extentions;
using theta_bot.Generators;

namespace theta_bot.Levels
{
    public class Level2 : ILevel
    {
        public bool IsFinished(IEnumerable<bool?> stats, long chatId) =>
            stats.Count() >= 5 &&
            stats.Reverse()
                .Take(5)
                .All(stat => stat != null && (bool)stat);
        
        private readonly IGenerator[] generators =
        {
            new ConstGenerator(), 
            new LogGenerator(), 
        };

        public Exercise Generate(Random random) =>
            new Exercise()
                .Generate(new ConstGenerator(), Tag.Code)
                .Generate(generators.Random(), new[] {Tag.While, Tag.For}.Random())
                .Generate(generators.Random(), new[] {Tag.While, Tag.For}.Random());
    }
}