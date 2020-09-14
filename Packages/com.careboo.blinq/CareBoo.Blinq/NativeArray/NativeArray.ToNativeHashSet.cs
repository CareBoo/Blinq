using System;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static NativeHashSet<T> ToNativeHashSet<T>(
            this ref NativeArray<T> source,
            Allocator allocator
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
