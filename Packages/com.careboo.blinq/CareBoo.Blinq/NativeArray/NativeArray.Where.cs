using Unity.Collections;
using CareBoo.Burst.Delegates;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<
            T,
            WhereSequence<T, NativeArraySequence<T>, NativeArray<T>.Enumerator, TPredicate>,
            WhereSequence<T, NativeArraySequence<T>, NativeArray<T>.Enumerator, TPredicate>.Enumerator>
        Where<T, TPredicate>(
            this in NativeArray<T> source,
            ValueFunc<T, int, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TPredicate : struct, IFunc<T, int, bool>
        {
            var sourceSeq = source.ToValueSequence();
            return sourceSeq.Where(predicate);
        }

        public static ValueSequence<
            T,
            WhereSequence<T, NativeArraySequence<T>, NativeArray<T>.Enumerator, IgnoreIndex<T, bool, TPredicate>>,
            WhereSequence<T, NativeArraySequence<T>, NativeArray<T>.Enumerator, IgnoreIndex<T, bool, TPredicate>>.Enumerator>
        Where<T, TPredicate>(
            this in NativeArray<T> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            var sourceSeq = source.ToValueSequence();
            return sourceSeq.Where(predicate);
        }
    }
}
