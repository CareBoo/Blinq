using System;
using System.Collections;
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

        NativeHashSet<T> union;

        public T Current { get; private set; }

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            Source.Dispose();
            Second.Dispose();
            if (union.IsCreated)
                union.Dispose();
        }

        public bool MoveNext()
        {
            if (!union.IsCreated)
                union = new NativeHashSet<T>(0, Allocator.Persistent);
            while (Source.MoveNext())
            {
                Current = Source.Current;
                if (union.Add(Current))
                    return true;
            }
            while (Second.MoveNext())
            {
                Current = Second.Current;
                if (union.Add(Current))
                    return true;
            }
            return false;
        }

        public void Reset()
        {
            if (union.IsCreated)
                union.Dispose();
            union = default;
            Current = default;
        }

        public NativeList<T> ToList()
        {
            var sourceList = Source.ToList();
            using (var secondList = Second.ToList())
            using (var set = new NativeHashSet<T>(sourceList.Length + secondList.Length, Allocator.Temp))
            {
                for (var i = 0; i < sourceList.Length; i++)
                    set.Add(sourceList[i]);
                for (var i = 0; i < secondList.Length; i++)
                    set.Add(secondList[i]);
                using (var setArr = set.ToNativeArray(Allocator.Temp))
                    sourceList.CopyFrom(setArr);
            }
            return sourceList;
        }
    }
}
