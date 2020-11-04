using Unity.Collections;
using CareBoo.Burst.Delegates;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<TResult, ZipSequence<T, NativeArraySequence<T>, TSecondElement, TSecond, TResult, TResultSelector>> Zip<T, TSecondElement, TSecond, TResult, TResultSelector>(
            this in NativeArray<T> source,
            in ValueSequence<TSecondElement, TSecond> secondSequence,
            in ValueFunc<T, TSecondElement, TResult>.Struct<TResultSelector> resultSelector
            )
            where T : struct
            where TSecondElement : struct
            where TSecond : struct, ISequence<TSecondElement>
            where TResult : struct
            where TResultSelector : struct, IFunc<T, TSecondElement, TResult>
        {
            var sourceSeq = source.ToValueSequence();
            return sourceSeq.Zip(in secondSequence, in resultSelector);
        }
    }
}
