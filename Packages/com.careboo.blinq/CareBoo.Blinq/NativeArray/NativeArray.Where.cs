using System;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class NativeArrayExtensions
    {
        public static ValueSequence<T, ValueSequence<T, NativeArraySequence<T>>.WhereWithIndexSequence<TPredicate>> Where<T, TPredicate>(
            this ref NativeArray<T> source,
            ValueFunc<T, int, bool>.Impl<TPredicate> predicate
            )
            where T : unmanaged, IEquatable<T>
            where TPredicate : struct, IFunc<T, int, bool>
        {
            return source.ToValueSequence().Where(predicate);
        }

        public static ValueSequence<T, ValueSequence<T, NativeArraySequence<T>>.WhereSequence<TPredicate>> Where<T, TPredicate>(
            this ref NativeArray<T> source,
            ValueFunc<T, bool>.Impl<TPredicate> predicate
            )
            where T : unmanaged, IEquatable<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            return source.ToValueSequence().Where(predicate);
        }
    }
}
