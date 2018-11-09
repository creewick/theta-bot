using System.Collections.Generic;

namespace ThetaBot.Extensions
{
    public static class StringExtensions
    {
        private static readonly Dictionary<int, string> Powers = new Dictionary<int, string>
        {
            {1, ""}, {2, "²"}, {3, "³"}
        };
        public static string InPower(this string argument, int power, string suffix="")
        {
            if (power == 0) return "";
            var powerStr = Powers.ContainsKey(power)
                ? Powers[power]
                : $"^{power}";

            return $"{argument}{powerStr}{suffix}";
        }
    }
}
