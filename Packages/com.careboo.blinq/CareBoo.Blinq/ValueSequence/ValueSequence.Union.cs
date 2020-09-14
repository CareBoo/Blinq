using System;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class ValueSequenceExtensions
    {
        public static ValueSequence<T, UnionSequence<T, TSource, TSecond>> Union<T, TSource, TSecond>(
            this ref ValueSequence<T, TSource> source,
            ValueSequence<T, TSecond> second
            )
            where T : unmanaged, IEquatable<T>
            where TSource : struct, ISequence<T>
            where TSecond : struct, ISequence<T>
        {
            var seq = new UnionSequence<T, TSource, TSecond> { Source = source.Source, Second = second.Source };
            return new ValueSequence<T, UnionSequence<T, TSource, TSecond>>(seq);
        }

        public static ValueSequence<T, UnionSequence<T, TSource, NativeArraySequence<T>>> Union<T, TSource>(
            this ref ValueSequence<T, TSource> source,
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

        public NativeList<T> Execute()
        {
            var sourceList = Source.Execute();
            var secondList = Second.Execute();
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
