using System;
using System.Collections.Generic;

namespace Theta_Bot.Extensions
{
    public static class EnumerableExtensions
    {
        public static T GetRandom<T>(this List<T> collection, Random random)
        {
            return collection[random.Next(collection.Count)];
        }
    }
}
