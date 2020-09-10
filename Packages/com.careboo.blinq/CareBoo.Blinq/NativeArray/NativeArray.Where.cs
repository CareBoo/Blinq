using System;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class NativeArrayExtensions
    {
        public static ValueSequence<T, ValueSequence<T, NativeArraySequence<T>>.WhereWithIndexSequence<TPredicate>> WhereWithIndex<T, TPredicate>(
            this ref NativeArray<T> source,
            TPredicate predicate = default
            )
            where T : unmanaged, IEquatable<T>
            where TPredicate : struct, IValueFunc<T, int, bool>
        {
            return source.ToValueSequence().WhereWithIndex(predicate);
        }

        public static ValueSequence<T, ValueSequence<T, NativeArraySequence<T>>.WhereSequence<TPredicate>> Where<T, TPredicate>(
            this ref NativeArray<T> source,
            TPredicate predicate = default
            )
            where T : unmanaged, IEquatable<T>
            where TPredicate : struct, IValueFunc<T, bool>
        {
            return source.ToValueSequence().Where(predicate);
        }
    }
}
