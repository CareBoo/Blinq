using System;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class NativeArrayExtensions
    {
        public static ValueSequence<TResult, ValueSequence<T, NativeArraySequence<T>>.ZipSequence<TSecondElement, TResult, TResultSelector, TSecond>> Zip<T, TSecondElement, TResult, TResultSelector, TSecond>(
            this ref NativeArray<T> source,
            TSecond secondSequence,
            TResultSelector resultSelector = default
            )
            where T : unmanaged, IEquatable<T>
            where TSecondElement : struct
            where TResult : unmanaged, IEquatable<TResult>
            where TResultSelector : struct, IValueFunc<T, TSecondElement, TResult>
            where TSecond : struct, ISequence<TSecondElement>
        {
            return source
                .ToValueSequence()
                .Zip<TSecondElement, TResult, TResultSelector, TSecond>(secondSequence, resultSelector);
        }
    }
}
