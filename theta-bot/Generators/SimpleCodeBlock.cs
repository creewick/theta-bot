using System;
using System.Text;
using Microsoft.CSharp;

namespace theta_bot
{
    public class SimpleCodeBlock : IGenerator
    {
        private readonly string[] templates =
        {
            "if ({0} % {1} == 0) count++;",
            "if ({0} < {1}) count++;",
            "if ({0} > {1}) count++;",
            "count++;"
        };

        public void ChangeCode(StringBuilder code, Func<Variable> getNextVar, Random random)
        {
            //Roslyn
            //CSharpCodeProvider.CreateProvider().CompileAssemblyFromSource().CompiledAssembly.GetType("MyType").GetMethod("Run").Invoke()
            var variable = getNextVar();
            var number = random.Next(10);
            var template = templates[random.Next(templates.Length)];
            
            code.AppendLine(string.Format(template, variable.Label, number));
        }
        
        public bool TryGetComplexity(Complexity oldComplexity, out Complexity newComplexity)
        {
            newComplexity = oldComplexity;
            return true;
        }
    }
}