using System;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class NativeArrayExtensions
    {
        public static ValueSequence<T, ValueSequence<T, NativeArraySequence<T>>.UnionSequence<TSecond>> Union<T, TSecond>(
           this ref NativeArray<T> source,
           TSecond second
           )
           where T : unmanaged, IEquatable<T>
           where TSecond : struct, ISequence<T>
        {
            return source.ToValueSequence().Union(second);
        }

        public static ValueSequence<T, ValueSequence<T, NativeArraySequence<T>>.UnionSequence<ValueSequence<T, NativeArraySequence<T>>>> Union<T>(
           this ref NativeArray<T> source,
           NativeArray<T> second
           )
           where T : unmanaged, IEquatable<T>
        {
            return source.ToValueSequence().Union(second);
        }

    }
}
