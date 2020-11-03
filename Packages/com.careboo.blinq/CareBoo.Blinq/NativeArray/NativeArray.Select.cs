using Unity.Collections;
using CareBoo.Burst.Delegates;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<TResult, SelectIndexSequence<T, NativeArraySequence<T>, TResult, TPredicate>> Select<T, TResult, TPredicate>(
            this in NativeArray<T> source,
            in ValueFunc<T, int, TResult>.Struct<TPredicate> selector
            )
            where T : struct
            where TResult : struct
            where TPredicate : struct, IFunc<T, int, TResult>
        {
            var sourceSeq = source.ToValueSequence();
            return sourceSeq.Select(in selector);
        }

        public static ValueSequence<TResult, SelectSequence<T, NativeArraySequence<T>, TResult, TPredicate>> Select<T, TResult, TPredicate>(
            this in NativeArray<T> source,
            in ValueFunc<T, TResult>.Struct<TPredicate> selector
            )
            where T : struct
            where TResult : struct
            where TPredicate : struct, IFunc<T, TResult>
        {
            var sourceSeq = source.ToValueSequence();
            return sourceSeq.Select(in selector);
        }
    }
}
