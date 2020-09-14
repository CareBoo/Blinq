using System;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static NativeList<T> ToNativeList<T>(
            this ref NativeArray<T> source,
            Allocator allocator
            )
            where T : unmanaged, IEquatable<T>
        {
            var result = new NativeList<T>(source.Length, allocator);
            result.CopyFrom(source);
            return result;
        }
    }
}
