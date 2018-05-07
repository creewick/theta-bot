using System;
using System.Collections.Generic;
using System.Linq;

namespace theta_bot.Extentions
{
    public static class EnumerableExtentions
    {
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> collection) => 
            collection
                .Select(x => new {Number = new Random().Next(), Item = x})
                .OrderBy(x => x.Number)
                .Select(x => x.Item);

        public static IEnumerable<T> Append<T>(this IEnumerable<T> collection, T element) => 
            collection.Concat(new[] {element});
    }
}