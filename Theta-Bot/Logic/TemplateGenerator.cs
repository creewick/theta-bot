using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Theta_Bot.Extensions;

namespace Theta_Bot.Logic
{
    public static class TemplateGenerator
    {
        private static readonly Random Random = new Random();
        private const string TokenRegex = @"\$.+?\$";

        public static string Generate(string template)
        {
            var parts = template.Split(Environment.NewLine + Environment.NewLine);
            var text = parts[0];
            var declaration = GetDeclaration(parts[1]);

            foreach (var token in GetTokens(text))
                text = text.Replace(token, GetValue(declaration[token]));

            return text;
        }

        private static IEnumerable<string> GetTokens(string text)
        {
            var matches = Regex.Matches(text, TokenRegex);
            for (var i = 0; i < matches.Count; i++)
                yield return matches[i].Value;
        }


        private static Dictionary<string, string> GetDeclaration(string lines)
        {
            return lines
                .Split(Environment.NewLine)
                .Select(line => line.Split(": "))
                .ToDictionary(match => $"${match[0]}$",
                              match => match[1]);
        }

        private static string GetValue(string request)
        {
            var match = Regex.Match(request, @"^one of \[(.+?)\]$");
            if (match.Success)
                return match.Groups[1].Value
                    .Split()
                    .ToList()
                    .GetRandom(Random);

            return request;
        }
    }
}
