using System.Collections.Generic;
using System.Linq;

namespace DLM.Inference
{
    internal static class StringJoinExtension
    {
        public static string JoinString<T>(this IEnumerable<T> collection, string separator)
        {
            return string.Join(separator, collection.Select(x => x.ToString()));
        }
    }
}
