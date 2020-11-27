using System;
using System.Collections.Generic;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static bool Contains<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            in T item
            )
            where T : struct, IEquatable<T>
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var sourceList = source.ToNativeList(Allocator.Temp);
            for (var i = 0; i < sourceList.Length; i++)
                if (sourceList[i].Equals(item))
                {
                    sourceList.Dispose();
                    return true;
                }
            sourceList.Dispose();
            return false;
        }

        public static bool Contains<T, TSource, TSourceEnumerator, TComparer>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            in T item,
            in TComparer comparer
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TComparer : struct, IEqualityComparer<T>
        {
            var sourceList = source.ToNativeList(Allocator.Temp);
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
