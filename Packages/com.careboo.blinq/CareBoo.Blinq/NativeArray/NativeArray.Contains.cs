using System;
using System.Collections.Generic;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class NativeArrayExtensions
    {
        public static bool Contains<T, TComparer>(this ref NativeArray<T> source, T item, TComparer comparer)
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
