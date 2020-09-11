using System;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class NativeArrayExtensions
    {
        public static ValueSequence<T, ValueSequence<T, NativeArraySequence<T>>.WhereWithIndexSequence> Where<T>(
            this ref NativeArray<T> source,
            ValueFunc<T, int, bool> predicate
            )
            where T : unmanaged, IEquatable<T>
        {
            return source.ToValueSequence().Where(predicate);
        }

        public static ValueSequence<T, ValueSequence<T, NativeArraySequence<T>>.WhereSequence> Where<T>(
            this ref NativeArray<T> source,
            ValueFunc<T, bool> predicate
            )
            where T : unmanaged, IEquatable<T>
        {
            return source.ToValueSequence().Where(predicate);
        }
    }
}
