using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<TResult, SelectIndexSequence<T, NativeArraySequence<T>, TResult, TPredicate>> Select<T, TResult, TPredicate>(
            this ref NativeArray<T> source,
            ValueFunc<T, int, TResult>.Impl<TPredicate> selector
            )
            where T : struct
            where TResult : struct
            where TPredicate : struct, IFunc<T, int, TResult>
        {
            return source.ToValueSequence().Select(selector);
        }

        public static ValueSequence<TResult, SelectSequence<T, NativeArraySequence<T>, TResult, TPredicate>> Select<T, TResult, TPredicate>(
            this ref NativeArray<T> source,
            ValueFunc<T, TResult>.Impl<TPredicate> selector
            )
            where T : struct
            where TResult : struct
            where TPredicate : struct, IFunc<T, TResult>
        {
            return source.ToValueSequence().Select(selector);
        }
    }
}
