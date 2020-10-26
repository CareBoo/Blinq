using System;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, UnionSequence<T, TSource, TSecond>> Union<T, TSource, TSecond>(
            this ValueSequence<T, TSource> source,
            ValueSequence<T, TSecond> second
            )
            where T : unmanaged, IEquatable<T>
            where TSource : struct, ISequence<T>
            where TSecond : struct, ISequence<T>
        {
            var seq = new UnionSequence<T, TSource, TSecond> { Source = source.Source, Second = second.Source };
            return ValueSequence<T>.New(seq);
        }

        public static ValueSequence<T, UnionSequence<T, TSource, NativeArraySequence<T>>> Union<T, TSource>(
            this ValueSequence<T, TSource> source,
            NativeArray<T> second
            )
            where T : unmanaged, IEquatable<T>
            where TSource : struct, ISequence<T>
        {
            return source.Union(second.ToValueSequence());
        }
    }

    public struct UnionSequence<T, TSource, TSecond> : ISequence<T>
        where T : unmanaged, IEquatable<T>
        where TSource : struct, ISequence<T>
        where TSecond : struct, ISequence<T>
    {
        public TSource Source;
        public TSecond Second;

        public NativeList<T> ToList()
        {
            var sourceList = Source.ToList();
            var secondList = Second.ToList();
            var set = new NativeHashSet<T>(sourceList.Length + secondList.Length, Allocator.Temp);
            for (var i = 0; i < sourceList.Length; i++)
                set.Add(sourceList[i]);
            for (var i = 0; i < secondList.Length; i++)
                set.Add(secondList[i]);
            var setArr = set.ToNativeArray(Allocator.Temp);
            sourceList.CopyFrom(setArr);
            secondList.Dispose();
            set.Dispose();
            setArr.Dispose();
            return sourceList;
        }
    }
}
