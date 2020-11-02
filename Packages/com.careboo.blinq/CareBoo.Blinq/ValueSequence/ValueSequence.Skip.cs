using System;
using System.Collections;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, SkipSequence<T, TSource>> Skip<T, TSource>(
            this ref ValueSequence<T, TSource> source,
            in int count
            )
            where T : struct
            where TSource : struct, ISequence<T>
        {
            var sourceSeq = source.GetEnumerator();
            var seq = new SkipSequence<T, TSource>(ref sourceSeq, count);
            return ValueSequence<T>.New(ref seq);
        }
    }

    public struct SkipSequence<T, TSource> : ISequence<T>
        where T : struct
        where TSource : struct, ISequence<T>
    {
        TSource source;
        int count;

        public SkipSequence(ref TSource source, int count)
        {
            this.source = source;
            this.count = count;
        }

        public T Current => source.Current;

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            source.Dispose();
        }

        public bool MoveNext()
        {
            while (count > 0)
            {
                count -= 1;
                source.MoveNext();
            }
            return source.MoveNext();
        }

        public void Reset()
        {
            throw new NotSupportedException();
        }

        public NativeList<T> ToList()
        {
            var list = source.ToList();
            if (count < list.Length)
                list.RemoveRangeWithBeginEnd(0, count);
            else
                list.Clear();
            return list;
        }
    }
}
