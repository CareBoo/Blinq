using System;
using System.Collections.Generic;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static bool Contains<T, TSource>(
            this ValueSequence<T, TSource> source,
            T item
            )
            where T : struct, IEquatable<T>
            where TSource : struct, ISequence<T>
        {
            var sourceList = source.Execute();
            for (var i = 0; i < sourceList.Length; i++)
                if (sourceList[i].Equals(item))
                    return true;
            return false;
        }

        public static bool Contains<T, TSource, TComparer>(
            this ValueSequence<T, TSource> source,
            T item,
            TComparer comparer
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TComparer : struct, IEqualityComparer<T>
        {
            var sourceList = source.Execute();
            for (var i = 0; i < sourceList.Length; i++)
                if (comparer.Equals(sourceList[i], item))
                    return true;
            return false;
        }
    }
}
