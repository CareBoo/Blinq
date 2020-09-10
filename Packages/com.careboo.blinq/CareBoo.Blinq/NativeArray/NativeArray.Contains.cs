using System;
using System.Collections.Generic;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class NativeArrayExtensions
    {
        public static bool Contains<T>(this ref NativeArray<T> source, T item)
            where T : unmanaged, IEquatable<T>
        {
            for (var i = 0; i < source.Length; i++)
                if (source[i].Equals(item))
                    return true;
            return false;
        }

        public static bool Contains<T, TComparer>(this ref NativeArray<T> source, T item, TComparer comparer)
            where T : unmanaged, IEquatable<T>
            where TComparer : struct, IEqualityComparer<T>
        {
            for (var i = 0; i < source.Length; i++)
                if (comparer.Equals(source[i], item))
                    return true;
            return false;
        }
    }
}
