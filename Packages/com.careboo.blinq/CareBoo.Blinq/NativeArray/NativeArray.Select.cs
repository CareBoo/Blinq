using System;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class NativeArrayExtensions
    {
        public static ValueSequence<TResult, ValueSequence<T, NativeArraySequence<T>>.SelectWithIndexSequence<TResult, TPredicate>> Select<T, TResult, TPredicate>(
            this ref NativeArray<T> source,
            ValueFunc<T, int, TResult>.Reference<TPredicate> selector
            )
            where T : unmanaged, IEquatable<T>
            where TResult : unmanaged, IEquatable<TResult>
            where TPredicate : struct, IFunc<T, int, TResult>
        {
            return source.ToValueSequence().Select(selector);
        }

        public static ValueSequence<TResult, ValueSequence<T, NativeArraySequence<T>>.SelectSequence<TResult, TPredicate>> Select<T, TResult, TPredicate>(
            this ref NativeArray<T> source,
            ValueFunc<T, TResult>.Reference<TPredicate> selector
            )
            where T : unmanaged, IEquatable<T>
            where TResult : unmanaged, IEquatable<TResult>
            where TPredicate : struct, IFunc<T, TResult>
        {
            return source.ToValueSequence().Select(selector);
        }
    }
}
