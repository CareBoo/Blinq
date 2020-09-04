using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class NativeArrayExtensions
    {
        public static ValueSequence<TResult, ValueSequence<T, NativeArraySourceQuery<T>>.ZipQuery<TSecond, TResult, TResultSelector, TSecondSequence>> Zip<T, TSecond, TResult, TResultSelector, TSecondSequence>(
            this ref NativeArray<T> source,
            TSecondSequence secondSequence,
            TResultSelector resultSelector = default
            )
            where T : struct
            where TSecond : struct
            where TResult : struct
            where TResultSelector : struct, IValueFunc<T, TSecond, TResult>
            where TSecondSequence : struct, IValueSequence<TSecond>
        {
            return source
                .ToValueSequence()
                .Zip<TSecond, TResult, TResultSelector, TSecondSequence>(secondSequence, resultSelector);
        }
    }
}
