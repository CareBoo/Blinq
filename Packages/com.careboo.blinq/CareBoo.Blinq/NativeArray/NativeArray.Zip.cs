using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<TResult, ZipSequence<T, NativeArraySequence<T>, TSecondElement, TResult, TSecond, TResultSelector>> Zip<T, TSecondElement, TResult, TSecond, TResultSelector>(
            this ref NativeArray<T> source,
            ValueSequence<TSecondElement, TSecond> secondSequence,
            ValueFunc<T, TSecondElement, TResult>.Impl<TResultSelector> resultSelector
            )
            where T : struct
            where TSecondElement : struct
            where TResult : struct
            where TSecond : struct, ISequence<TSecondElement>
            where TResultSelector : struct, IFunc<T, TSecondElement, TResult>
        {
            var seq = source.ToValueSequence();
            return seq.Zip(secondSequence, resultSelector);
        }
    }
}
