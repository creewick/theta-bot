using System.Collections.Generic;

namespace theta_bot.Extentions
{
    public static class IntExtenstion
    {
        private static readonly Dictionary<int, string> Powers = new Dictionary<int, string>
        {
            {0, ""}, {1, ""}, {2, "²"}, {3, "³"}
        };
        public static string ToPower(this int number)
        {
            return Powers.ContainsKey(number) 
                ? Powers[number] 
                : $"^{number}";
        }
    }
}