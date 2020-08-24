using System;
using System.Collections.Generic;
using Unity.Collections;
using LinqEnumerable = System.Linq.Enumerable;

internal static class EnumerableExtensions
{
    public static NativeArray<T> ToNativeArray<T>(this IEnumerable<T> source, Allocator allocator)
        where T : unmanaged
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        return new NativeArray<T>(LinqEnumerable.ToArray(source), allocator);
    }
}
