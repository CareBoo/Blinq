using System;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static NativeHashSet<T> ToNativeHashSet<T, TSource>(
            this in ValueSequence<T, TSource> source,
            in Allocator allocator
            )
            where T : unmanaged, IEquatable<T>
            where TSource : struct, ISequence<T>
        {
            var list = source.Execute();
            var result = new NativeHashSet<T>(list.Length, allocator);
            for (var i = 0; i < list.Length; i++)
                result.Add(list[i]);
            list.Dispose();
            return result;
        }
    }
}
