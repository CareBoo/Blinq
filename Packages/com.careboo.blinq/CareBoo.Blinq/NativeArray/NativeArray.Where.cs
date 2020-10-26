using Unity.Collections;
using CareBoo.Burst.Delegates;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, WhereIndexSequence<T, NativeArraySequence<T>, TPredicate>> Where<T, TPredicate>(
            this ref NativeArray<T> source,
            ValueFunc<T, int, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TPredicate : struct, IFunc<T, int, bool>
        {
            return source.ToValueSequence().Where(predicate);
        }

        public static ValueSequence<T, WhereSequence<T, NativeArraySequence<T>, TPredicate>> Where<T, TPredicate>(
            this ref NativeArray<T> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            return source.ToValueSequence().Where(predicate);
        }
    }
}
