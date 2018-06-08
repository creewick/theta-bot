using System;
using System.Collections.Generic;
using System.Linq;
using Firebase.Database.Offline;
using theta_bot.Classes;
using theta_bot.Extentions;

namespace theta_bot.Generators
{
    public abstract class Generator
    {
        private static readonly string[] Operations = {"+", "-", "*", "/"};
        private static readonly string[] PositiveOperations = {"+", "*"};
        
        protected static string GetRandomOperation(Random random)
            => Operations.Random(random);
        
        /* {0} cycle variable
         * {1} start value
         * {2} end value
         * {3} operation
         * {4} step value
         */
        private static readonly string[] ForPositive =
        {
            "for (var {0}={1}; {0}<{2}; {0}{3}={4})\n{{\n",
            "for (var {0}={1}; {0}<{2}; {0}={0}{3}{4})\n{{\n"
        };
        
        private static readonly string[] ForNegative = 
        {
            "for (var {0}={2}; {0}>{1}; {0}{3}={4})\n{{\n",
            "for (var {0}={2}; {0}>{1}; {0}={0}{3}{4})\n{{\n",
        };

        private static readonly string[] WhileTemplates =
        {
            "var {0} = {1};\nwhile ({0} < {2})\n{{\n    {0} {3}= {4};\n",
            "var {0} = {1};\nwhile ({0} < {2})\n{{\n    {0} = {0} {3} {4};\n",
            "var {0} = {2};\nwhile ({0} < {1})\n{{\n    {0} {3}= {4};\n",
            "var {0} = {2};\nwhile ({0} > {1})\n{{\n    {0} = {0} {3} {4};\n",
        };

        protected static string GetRandomTemplate(Random random, string operation, LoopType[] loopTypes)
        {
            if (!Operations.Contains(operation))
                throw new ArgumentException("Unknown operation");
            
            
            var forTag = loopTypes.Contains(LoopType.For);
            var whileTag = loopTypes.Contains(LoopType.While);
            if (forTag && whileTag)
                throw new ArgumentException("Generator called with conflicting tags");
            if (!forTag && !whileTag)
                throw  new ArgumentException("Generator called without required tag");
            return forTag 
                ? ForTemplates.Random(random) 
                : WhileTemplates.Random(random);
        }

        private static Dictionary<VarType, Func<Random, string>> RandomBound = 
            new Dictionary<VarType, Func<Random, string>>
        {
            [ VarType.Const, r => r.Next(1, 5) * 100; ],
            [ VarType.N, r => "n" ],
            [ VarType.I, r => "i" ]
        };
        
        private static Dictionary<LoopStepType, Func<Random, string>> randomStep =
            new Dictionary<LoopStepType, Func<Random, string>>
        {
            [ LoopStepType.IncConst, r => ]    
        };

        protected string GetRandomBound(Random random, VarType type) 
            => RandomBound[type](random);
    }
}