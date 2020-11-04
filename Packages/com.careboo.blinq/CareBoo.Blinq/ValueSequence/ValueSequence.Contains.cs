using System;
using System.Collections.Generic;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static bool Contains<T, TSource>(
            this in ValueSequence<T, TSource> source,
            in T item
            )
            where T : struct, IEquatable<T>
            where TSource : struct, ISequence<T>
        {
            var sourceList = source.Execute();
            for (var i = 0; i < sourceList.Length; i++)
                if (sourceList[i].Equals(item))
                {
                    sourceList.Dispose();
                    return true;
                }
            sourceList.Dispose();
            return false;
        }

        public static bool Contains<T, TSource, TComparer>(
            this in ValueSequence<T, TSource> source,
            in T item,
            in TComparer comparer
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TComparer : struct, IEqualityComparer<T>
        {
            var sourceList = source.Execute();
            for (var i = 0; i < sourceList.Length; i++)
                if (comparer.Equals(sourceList[i], item))
                {
                    sourceList.Dispose();
                    return true;
                }
            sourceList.Dispose();
            return false;
        }
    }
}
