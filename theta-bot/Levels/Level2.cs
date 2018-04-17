﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace theta_bot
{
    public class Level2 : ILevel
    {
        public bool IsFinished(IEnumerable<bool?> stats, long chatId) =>
            stats.Count() >= 5 &&
            stats.Reverse()
                .Take(5)
                .All(stat => stat != null && (bool)stat);
        
        private readonly SimpleCodeBlock simpleCode = new SimpleCodeBlock();
        private readonly IGenerator[] loopGenerators =
        {
            new SimpleForLoop(),
            new LinearForLoop(),
            new LogarithmicForLoop(),
            new SimpleWhileLoop(), 
            new LinearWhileLoop(), 
        };
        
        public Exercise Generate(Random random)
        {
            var i = random.Next(loopGenerators.Length);
            var j = random.Next(loopGenerators.Length);
            return new Exercise()
                .Generate(simpleCode, random)
                .Generate(loopGenerators[i], random)
                .Generate(loopGenerators[j], random)
                .BoundVars();
        }
    }
}