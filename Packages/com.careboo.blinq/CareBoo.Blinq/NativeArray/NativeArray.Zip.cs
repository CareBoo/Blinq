using System;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class NativeArrayExtensions
    {
        public static ValueSequence<TResult, ValueSequence<T, NativeArraySequence<T>>.ZipSequence<TSecondElement, TResult, TSecond, TResultSelector>> Zip<T, TSecondElement, TResult, TSecond, TResultSelector>(
            this ref NativeArray<T> source,
            TSecond secondSequence,
            ValueFunc<T, TSecondElement, TResult>.Impl<TResultSelector> resultSelector
            )
            where T : unmanaged, IEquatable<T>
            where TSecondElement : struct
            where TResult : unmanaged, IEquatable<TResult>
            where TSecond : struct, ISequence<TSecondElement>
            where TResultSelector : struct, IFunc<T, TSecondElement, TResult>
        {
            return source
                .ToValueSequence()
                .Zip(secondSequence, resultSelector);
        }
    }
}
