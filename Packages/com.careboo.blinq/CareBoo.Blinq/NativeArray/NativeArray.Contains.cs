using System;
using System.Collections.Generic;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static bool Contains<T>(
            this in NativeArray<T> source,
            in T item
            )
            where T : struct, IEquatable<T>
        {
            for (var i = 0; i < source.Length; i++)
                if (source[i].Equals(item))
                    return true;
            return false;
        }

        public static bool Contains<T, TComparer>(
            this in NativeArray<T> source,
            in T item,
            in TComparer comparer
            )
            where T : struct
            where TComparer : struct, IEqualityComparer<T>
        {
            for (var i = 0; i < source.Length; i++)
                if (comparer.Equals(source[i], item))
                    return true;
            return false;
        }
    }
}
