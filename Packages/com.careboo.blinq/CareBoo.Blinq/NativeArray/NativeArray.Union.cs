using System;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, UnionSequence<T, NativeArraySequence<T>, TSecond>> Union<T, TSecond>(
           this in NativeArray<T> source,
           in ValueSequence<T, TSecond> second
           )
           where T : unmanaged, IEquatable<T>
           where TSecond : struct, ISequence<T>
        {
            var sourceSeq = source.ToValueSequence();
            return sourceSeq.Union(in second);
        }

        public static ValueSequence<T, UnionSequence<T, NativeArraySequence<T>, NativeArraySequence<T>>> Union<T>(
           this in NativeArray<T> source,
           in NativeArray<T> second
           )
           where T : unmanaged, IEquatable<T>
        {
            var sourceSeq = source.ToValueSequence();
            return sourceSeq.Union(in second);
        }
    }
}
