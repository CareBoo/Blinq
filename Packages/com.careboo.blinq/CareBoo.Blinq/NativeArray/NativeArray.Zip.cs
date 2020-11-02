using Unity.Collections;
using CareBoo.Burst.Delegates;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<TResult, ZipSequence<T, NativeArraySequence<T>, TSecondElement, TSecond, TResult, TResultSelector>> Zip<T, TSecondElement, TSecond, TResult, TResultSelector>(
            this ref NativeArray<T> source,
            ValueSequence<TSecondElement, TSecond> secondSequence,
            ValueFunc<T, TSecondElement, TResult>.Struct<TResultSelector> resultSelector
            )
            where T : struct
            where TSecondElement : struct
            where TSecond : struct, ISequence<TSecondElement>
            where TResult : struct
            where TResultSelector : struct, IFunc<T, TSecondElement, TResult>
        {
            return source.ToValueSequence().Zip(secondSequence, resultSelector);
        }
    }
}
