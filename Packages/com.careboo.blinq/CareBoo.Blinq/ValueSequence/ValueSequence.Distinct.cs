using System;
using System.Collections;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, DistinctSequence<T, TSource>> Distinct<T, TSource>(
            this ValueSequence<T, TSource> source
            )
            where T : unmanaged, IEquatable<T>
            where TSource : struct, ISequence<T>
        {
            var seq = new DistinctSequence<T, TSource> { Source = source.Source };
            return ValueSequence<T>.New(seq);
        }
    }

    public struct DistinctSequence<T, TSource> : ISequence<T>
        where T : unmanaged, IEquatable<T>
        where TSource : struct, ISequence<T>
    {
        public TSource Source;

        public T Current => Source.Current;

        NativeHashSet<T> set;

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            Source.Dispose();
            if (set.IsCreated)
                set.Dispose();
            set = default;
        }

        public bool MoveNext()
        {
            if (!set.IsCreated)
                set = new NativeHashSet<T>(1, Allocator.Persistent);
            while (Source.MoveNext())
                if (set.Add(Source.Current))
                    return true;
            return false;
        }

        public void Reset()
        {
            Source.Reset();
            if (set.IsCreated)
                set.Dispose();
            set = default;
        }

        public NativeList<T> ToList()
        {
            var list = Source.ToList();
            using (var tempSet = new NativeHashSet<T>(list.Length, Allocator.Temp))
            {
                for (var i = 0; i < list.Length; i++)
                    if (!tempSet.Add(list[i]))
                    {
                        list.RemoveAt(i);
                        i--;
                    }
                return list;
            }
        }
    }
}
