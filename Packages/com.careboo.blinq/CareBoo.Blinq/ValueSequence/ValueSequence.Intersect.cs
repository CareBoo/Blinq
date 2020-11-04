using System;
using System.Collections;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, IntersectSequence<T, TSource, TSecond>> Intersect<T, TSource, TSecond>(
            this in ValueSequence<T, TSource> source,
            in ValueSequence<T, TSecond> second
            )
            where T : unmanaged, IEquatable<T>
            where TSource : struct, ISequence<T>
            where TSecond : struct, ISequence<T>
        {
            var sourceSeq = source.GetEnumerator();
            var secondSeq = second.GetEnumerator();
            var seq = new IntersectSequence<T, TSource, TSecond>(ref sourceSeq, ref secondSeq);
            return ValueSequence<T>.New(ref seq);
        }

        public static ValueSequence<T, IntersectSequence<T, TSource, NativeArraySequence<T>>> Intersect<T, TSource>(
            this in ValueSequence<T, TSource> source,
            in NativeArray<T> second
            )
            where T : unmanaged, IEquatable<T>
            where TSource : struct, ISequence<T>
        {
            return source.Intersect(second.ToValueSequence());
        }
    }

    public struct IntersectSequence<T, TSource, TSecond> : ISequence<T>
        where T : unmanaged, IEquatable<T>
        where TSource : struct, ISequence<T>
        where TSecond : struct, ISequence<T>
    {
        TSource source;
        TSecond second;
        NativeHashSet<T> intersection;
        NativeHashSet<T> added;

        public IntersectSequence(ref TSource source, ref TSecond second)
        {
            this.source = source;
            this.second = second;
            intersection = default;
            added = default;
        }

        public T Current => second.Current;

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            source.Dispose();
            second.Dispose();
            if (intersection.IsCreated)
                intersection.Dispose();
            if (added.IsCreated)
                added.Dispose();
        }

        public bool MoveNext()
        {
            if (!intersection.IsCreated)
            {
                var sourceList = source.ToList();
                intersection = new NativeHashSet<T>(sourceList.Length, Allocator.Persistent);
                for (var i = 0; i < sourceList.Length; i++)
                    intersection.Add(sourceList[i]);
            }
            if (!added.IsCreated)
                added = new NativeHashSet<T>(0, Allocator.Persistent);
            while (second.MoveNext())
                if (intersection.Contains(second.Current) && added.Add(second.Current))
                    return true;
            return false;
        }

        public void Reset()
        {
            throw new NotSupportedException();
        }

        public NativeList<T> ToList()
        {
            var sourceList = source.ToList();
            var secondList = second.ToList();
            var sourceSet = new NativeHashSet<T>(sourceList.Length, Allocator.Temp);
            var secondSet = new NativeHashSet<T>(secondList.Length, Allocator.Temp);
            var result = new NativeList<T>(secondList.Length, Allocator.Temp);
            for (var i = 0; i < sourceList.Length; i++)
                sourceSet.Add(sourceList[i]);
            for (var i = 0; i < secondList.Length; i++)
                if (sourceSet.Contains(secondList[i]) && secondSet.Add(secondList[i]))
                    result.AddNoResize(secondList[i]);
            sourceList.Dispose();
            secondList.Dispose();
            sourceSet.Dispose();
            secondSet.Dispose();
            return result;
        }
    }
}
