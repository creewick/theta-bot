﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using theta_bot.Classes.Enums;
using theta_bot.Extentions;

namespace theta_bot.Logic
{
    public class Loop
    {
        public readonly OpType Operation;
        public readonly VarType Bound;
        public readonly VarType Step;

        public Loop(VarType bound, OpType operation, VarType step)
        {
            Operation = operation;
            Bound = bound;
            Step = step;
        }
        
        public static Loop Random(Random random) =>
            new Loop(VarTypes.Random(random), OpTypes.Random(random), VarTypes.Random(random));

        private static readonly IEnumerable<OpType> OpTypes =
            Enum.GetValues(typeof(OpType)).Cast<OpType>();

        private static readonly IEnumerable<VarType> VarTypes =
            Enum.GetValues(typeof(VarType)).Cast<VarType>();
    }
}