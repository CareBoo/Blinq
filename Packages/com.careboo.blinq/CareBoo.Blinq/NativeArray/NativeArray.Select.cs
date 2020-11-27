using Unity.Collections;
using CareBoo.Burst.Delegates;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<
            TResult,
            SelectSequence<T, NativeArraySequence<T>, NativeArray<T>.Enumerator, TResult, TPredicate>,
            SelectSequence<T, NativeArraySequence<T>, NativeArray<T>.Enumerator, TResult, TPredicate>.Enumerator>
        Select<T, TResult, TPredicate>(
            this in NativeArray<T> source,
            ValueFunc<T, int, TResult>.Struct<TPredicate> selector
            )
            where T : struct
            where TResult : struct
            where TPredicate : struct, IFunc<T, int, TResult>
        {
            var sourceSeq = source.ToValueSequence();
            return sourceSeq.Select(selector);
        }

        public static ValueSequence<
            TResult,
            SelectSequence<T, NativeArraySequence<T>, NativeArray<T>.Enumerator, TResult, IgnoreIndex<T, TResult, TPredicate>>,
            SelectSequence<T, NativeArraySequence<T>, NativeArray<T>.Enumerator, TResult, IgnoreIndex<T, TResult, TPredicate>>.Enumerator>
        Select<T, TResult, TPredicate>(
            this in NativeArray<T> source,
            ValueFunc<T, TResult>.Struct<TPredicate> selector
            )
            where T : struct
            where TResult : struct
            where TPredicate : struct, IFunc<T, TResult>
        {
            var sourceSeq = source.ToValueSequence();
            return sourceSeq.Select(selector);
        }
    }
}
