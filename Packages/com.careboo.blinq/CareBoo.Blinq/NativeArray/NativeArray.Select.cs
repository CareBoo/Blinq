using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class NativeArrayExtensions
    {
        public static ValueSequence<TResult, ValueSequence<T, NativeArraySourceQuery<T>>.SelectWithIndexQuery<TResult, TSelector>> SelectWithIndex<T, TResult, TSelector>(
            this ref NativeArray<T> source,
            TSelector selector = default
            )
            where T : struct
            where TResult : struct
            where TSelector : struct, IValueFunc<T, int, TResult>
        {
            return source.ToValueSequence().SelectWithIndex<TResult, TSelector>(selector);
        }

        public static ValueSequence<TResult, ValueSequence<T, NativeArraySourceQuery<T>>.SelectQuery<TResult, TSelector>> Select<T, TResult, TSelector>(
            this ref NativeArray<T> source,
            TSelector selector = default
            )
            where T : struct
            where TResult : struct
            where TSelector : struct, IValueFunc<T, TResult>
        {
            return source.ToValueSequence().Select<TResult, TSelector>(selector);
        }
    }
}
