using System;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static NativeHashSet<T> ToNativeHashSet<T>(
            this in NativeArray<T> source,
            in Allocator allocator
            )
            where T : unmanaged, IEquatable<T>
        {
            var result = new NativeHashSet<T>(source.Length, allocator);
            for (var i = 0; i < source.Length; i++)
                result.Add(source[i]);
            return result;
        }
    }
}
