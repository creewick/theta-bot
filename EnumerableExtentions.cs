using System;
using System.Collections.Generic;
using System.Linq;

namespace theta_bot
{
    public static class EnumerableExtentions
    {
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> collection, Random random)
        {
            return collection
                .Select(x => new {Number = random.Next(), Item = x})
                .OrderBy(x => x.Number)
                .Select(x => x.Item);
        }

        public static IEnumerable<T> Append<T>(this IEnumerable<T> collection, T element)
        {
            return collection.Concat(new[] {element});
        }
    }
}