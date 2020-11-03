using Unity.Collections;
using CareBoo.Burst.Delegates;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, WhereIndexSequence<T, NativeArraySequence<T>, TPredicate>> Where<T, TPredicate>(
            this in NativeArray<T> source,
            in ValueFunc<T, int, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TPredicate : struct, IFunc<T, int, bool>
        {
            var sourceSeq = source.ToValueSequence();
            return sourceSeq.Where(in predicate);
        }

        public static ValueSequence<T, WhereSequence<T, NativeArraySequence<T>, TPredicate>> Where<T, TPredicate>(
            this in NativeArray<T> source,
            in ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            var sourceSeq = source.ToValueSequence();
            return sourceSeq.Where(in predicate);
        }
    }
}
