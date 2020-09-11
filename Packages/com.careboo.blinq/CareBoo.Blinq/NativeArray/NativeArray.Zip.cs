using System;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class NativeArrayExtensions
    {
        public static ValueSequence<TResult, ValueSequence<T, NativeArraySequence<T>>.ZipSequence<TSecondElement, TResult, TSecond>> Zip<T, TSecondElement, TResult, TSecond>(
            this ref NativeArray<T> source,
            TSecond secondSequence,
            ValueFunc<T, TSecondElement, TResult> resultSelector
            )
            where T : unmanaged, IEquatable<T>
            where TSecondElement : struct
            where TResult : unmanaged, IEquatable<TResult>
            where TSecond : struct, ISequence<TSecondElement>
        {
            return source
                .ToValueSequence()
                .Zip(secondSequence, resultSelector);
        }
    }
}
