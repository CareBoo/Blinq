using Unity.Collections;
using CareBoo.Burst.Delegates;
using System.Collections.Generic;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<
            TResult,
            ZipSequence<T, NativeArraySequence<T>, NativeArray<T>.Enumerator, TSecondElement, TSecond, TSecondEnumerator, TResult, TResultSelector>,
            ZipSequence<T, NativeArraySequence<T>, NativeArray<T>.Enumerator, TSecondElement, TSecond, TSecondEnumerator, TResult, TResultSelector>.Enumerator>
        Zip<T, TSecondElement, TSecond, TSecondEnumerator, TResult, TResultSelector>(
            this in NativeArray<T> source,
            in ValueSequence<TSecondElement, TSecond, TSecondEnumerator> secondSequence,
            ValueFunc<T, TSecondElement, TResult>.Struct<TResultSelector> resultSelector
            )
            where T : struct
            where TSecondElement : struct
            where TSecond : struct, ISequence<TSecondElement, TSecondEnumerator>
            where TSecondEnumerator : struct, IEnumerator<TSecondElement>
            where TResult : struct
            where TResultSelector : struct, IFunc<T, TSecondElement, TResult>
        {
            var sourceSeq = source.ToValueSequence();
            return sourceSeq.Zip(in secondSequence, resultSelector);
        }
    }
}
