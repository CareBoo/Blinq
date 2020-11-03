using System;
using System.Collections;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, UnionSequence<T, TSource, TSecond>> Union<T, TSource, TSecond>(
            this in ValueSequence<T, TSource> source,
            in ValueSequence<T, TSecond> second
            )
            where T : unmanaged, IEquatable<T>
            where TSource : struct, ISequence<T>
            where TSecond : struct, ISequence<T>
        {
            var sourceSeq = source.GetEnumerator();
            var secondSeq = second.GetEnumerator();
            var seq = new UnionSequence<T, TSource, TSecond>(ref sourceSeq, ref secondSeq);
            return ValueSequence<T>.New(ref seq);
        }

        public static ValueSequence<T, UnionSequence<T, TSource, NativeArraySequence<T>>> Union<T, TSource>(
            this in ValueSequence<T, TSource> source,
            in NativeArray<T> second
            )
            where T : unmanaged, IEquatable<T>
            where TSource : struct, ISequence<T>
        {
            var secondSeq = second.ToValueSequence();
            return source.Union(in secondSeq);
        }
    }

    public struct UnionSequence<T, TSource, TSecond> : ISequence<T>
        where T : unmanaged, IEquatable<T>
        where TSource : struct, ISequence<T>
        where TSecond : struct, ISequence<T>
    {
        public TSource source;
        public TSecond second;

        NativeHashSet<T> union;

        public UnionSequence(ref TSource source, ref TSecond second)
        {
            this.source = source;
            this.second = second;
            union = default;
            Current = default;
        }

        public T Current { get; private set; }

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            source.Dispose();
            second.Dispose();
            union.Dispose();
        }

        public bool MoveNext()
        {
            if (!union.IsCreated)
                union = new NativeHashSet<T>(0, Allocator.Persistent);
            while (source.MoveNext())
            {
                Current = source.Current;
                if (union.Add(Current))
                    return true;
            }
            while (second.MoveNext())
            {
                Current = second.Current;
                if (union.Add(Current))
                    return true;
            }
            return false;
        }

        public void Reset()
        {
            throw new NotSupportedException();
        }

        public NativeList<T> ToList()
        {
            var sourceList = source.ToList();
            using (var secondList = second.ToList())
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
