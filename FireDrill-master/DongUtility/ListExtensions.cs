using System;
using System.Collections.Generic;

namespace DongUtility
{
    public static class ListExtensions
    {
        /// <summary>
        /// Randomly rearranges a list
        /// </summary>
        public static void Shuffle<T>(this IList<T> list, Random generator)
        {
            int n = list.Count;
            while (n > 1)
            {
                --n;
                int k = generator.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static T PickRandom<T>(this IList<T> list, Random generator)
        {
            int k = generator.Next(list.Count);
            return list[k];
        }
    }
}
