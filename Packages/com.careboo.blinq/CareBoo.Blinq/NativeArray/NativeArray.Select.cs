using System;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class NativeArrayExtensions
    {
        public static ValueSequence<TResult, ValueSequence<T, NativeArraySequence<T>>.SelectWithIndexSequence<TResult>> Select<T, TResult>(
            this ref NativeArray<T> source,
            ValueFunc<T, int, TResult> selector
            )
            where T : unmanaged, IEquatable<T>
            where TResult : unmanaged, IEquatable<TResult>
        {
            return source.ToValueSequence().Select(selector);
        }

        public static ValueSequence<TResult, ValueSequence<T, NativeArraySequence<T>>.SelectSequence<TResult>> Select<T, TResult>(
            this ref NativeArray<T> source,
            ValueFunc<T, TResult> selector
            )
            where T : unmanaged, IEquatable<T>
            where TResult : unmanaged, IEquatable<TResult>
        {
            return source.ToValueSequence().Select(selector);
        }
    }
}
