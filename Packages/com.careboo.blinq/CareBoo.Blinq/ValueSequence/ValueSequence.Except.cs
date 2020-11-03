using System;
using System.Collections;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, ExceptSequence<T, TSource, TSecond>> Except<T, TSource, TSecond>(
            this in ValueSequence<T, TSource> source,
            in ValueSequence<T, TSecond> second
            )
            where T : unmanaged, IEquatable<T>
            where TSource : struct, ISequence<T>
            where TSecond : struct, ISequence<T>
        {
            var sourceSeq = source.GetEnumerator();
            var secondSeq = second.GetEnumerator();
            var seq = new ExceptSequence<T, TSource, TSecond>(ref sourceSeq, ref secondSeq);
            return ValueSequence<T>.New(ref seq);
        }

        public static ValueSequence<T, ExceptSequence<T, TSource, NativeArraySequence<T>>> Except<T, TSource>(
            this in ValueSequence<T, TSource> source,
            in NativeArray<T> second
            )
            where T : unmanaged, IEquatable<T>
            where TSource : struct, ISequence<T>
        {
            return source.Except(second.ToValueSequence());
        }
    }

    public struct ExceptSequence<T, TSource, TSecond> : ISequence<T>
        where T : unmanaged, IEquatable<T>
        where TSource : struct, ISequence<T>
        where TSecond : struct, ISequence<T>
    {
        public TSource source;
        public TSecond second;

        NativeHashSet<T> set;

        public ExceptSequence(ref TSource source, ref TSecond second)
        {
            this.source = source;
            this.second = second;
            set = default;
        }

        public T Current => source.Current;

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
                using (var secondList = second.ToList())
                {
                    set = new NativeHashSet<T>(secondList.Length, Allocator.Persistent);
                    for (var i = 0; i < secondList.Length; i++)
                        set.Add(secondList[i]);
                }
            }
            while (source.MoveNext())
                if (!set.Contains(source.Current))
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
            using (var secondList = second.ToList())
            using (var secondSet = new NativeHashSet<T>(secondList.Length, Allocator.Temp))
            {
                for (var i = 0; i < secondList.Length; i++)
                    secondSet.Add(secondList[i]);
                for (var i = 0; i < sourceList.Length; i++)
                    if (!secondSet.Add(sourceList[i]))
                    {
                        sourceList.RemoveAt(i);
                        i--;
                    }
                return sourceList;
            }
        }
    }
}
