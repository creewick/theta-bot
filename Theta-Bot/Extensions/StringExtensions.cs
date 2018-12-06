using System;

namespace Theta_Bot.Extensions
{
    public static class StringExtensions
    {
        public static string[] Split(this string text, string separator) =>
            text.Split(new [] {separator}, StringSplitOptions.None);
    }
}
