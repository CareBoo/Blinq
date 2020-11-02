using System;
using System.Collections;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, IntersectSequence<T, TSource, TSecond>> Intersect<T, TSource, TSecond>(
            this ref ValueSequence<T, TSource> source,
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
            this ref ValueSequence<T, TSource> source,
            ref NativeArray<T> second
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
        NativeHashSet<T> set;

        public IntersectSequence(ref TSource source, ref TSecond second)
        {
            this.source = source;
            this.second = second;
            set = default;
        }

        public T Current => second.Current;

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            source.Dispose();
            second.Dispose();
            set.Dispose();
        }

        public bool MoveNext()
        {
            if (!set.IsCreated)
            {
                var sourceList = source.ToList();
                set = new NativeHashSet<T>(sourceList.Length, Allocator.Persistent);
                for (var i = 0; i < sourceList.Length; i++)
                    set.Add(sourceList[i]);
            }
            while (second.MoveNext())
                if (set.Contains(second.Current))
                    return true;
            return false;
        }

        public void Reset()
        {
            throw new NotSupportedException();
        }

        public NativeList<T> ToList()
        {
            using (var sourceList = source.ToList())
            using (var secondList = second.ToList())
            using (var sourceSet = new NativeHashSet<T>(sourceList.Length, Allocator.Temp))
            using (var secondSet = new NativeHashSet<T>(secondList.Length, Allocator.Temp))
            {
                var result = new NativeList<T>(secondList.Length, Allocator.Temp);
                for (var i = 0; i < sourceList.Length; i++)
                    sourceSet.Add(sourceList[i]);
                for (var i = 0; i < secondList.Length; i++)
                    if (sourceSet.Contains(secondList[i]) && secondSet.Add(secondList[i]))
                        result.AddNoResize(secondList[i]);
                return result;
            }
        }
    }
}
