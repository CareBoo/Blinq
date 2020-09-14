using System;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, WhereIndexSequence<T, NativeArraySequence<T>, TPredicate>> Where<T, TPredicate>(
            this ref NativeArray<T> source,
            ValueFunc<T, int, bool>.Impl<TPredicate> predicate
            )
            where T : struct
            where TPredicate : struct, IFunc<T, int, bool>
        {
            var seq = source.ToValueSequence();
            return seq.Where(predicate);
        }

        public static ValueSequence<T, WhereSequence<T, NativeArraySequence<T>, TPredicate>> Where<T, TPredicate>(
            this ref NativeArray<T> source,
            ValueFunc<T, bool>.Impl<TPredicate> predicate
            )
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            var seq = source.ToValueSequence();
            return seq.Where(predicate);
        }
    }
}
