﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace BrokerAlgo.Helpers
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> source, int n)
        {
            return source.Skip(Math.Max(0, source.Count() - n));
        }

        public static IEnumerable<T> ExceptLast<T>(this IEnumerable<T> source)
        {
            bool isFirst = true;
            T item = default(T);

            var it = source.GetEnumerator();
            while (it.MoveNext())
            {
                if (!isFirst)
                    yield return item;
                item = it.Current;
                isFirst = false;
            }
        }
    }
}