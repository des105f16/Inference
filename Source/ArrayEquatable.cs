using System;
using System.Collections.Generic;
using System.Linq;

namespace DLM.Inference
{
    internal static class ArrayEquatable
    {
        public static bool Equals<T>(T[] array1, T[] array2) where T : IEquatable<T>
        {
            if (array1 == null && array2 == null)
                return true;
            if (array1 == null || array2 == null)
                return false;

            if (array1.Length == 0 && array2.Length == 0)
                return true;
            if (array1.Length == 0 || array2.Length == 0)
                return false;

            var match = new List<T>(array2);
            for (int i = 0; i < array1.Length; i++)
            {
                bool found = false;

                for (int j = 0; j < match.Count; j++)
                    if (array1[i].Equals(match[j]))
                    {
                        match.RemoveAt(j--);
                        found = true;
                    }

                if (!found)
                    return false;
            }

            return match.Count == 0;
        }
        public static bool Equals<T>(IEnumerable<T> collection1, IEnumerable<T> collection2) where T : IEquatable<T>
        {
            return Equals(collection1.ToArray(), collection2.ToArray());
        }
    }
}
