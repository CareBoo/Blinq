using System;
using System.Collections;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, TakeSequence<T, TSource>> Take<T, TSource>(
            this in ValueSequence<T, TSource> source,
            in int count
            )
            where T : struct
            where TSource : struct, ISequence<T>
        {
            var sourceSeq = source.GetEnumerator();
            var seq = new TakeSequence<T, TSource>(ref sourceSeq, count);
            return ValueSequence<T>.New(ref seq);
        }
    }

    public struct TakeSequence<T, TSource> : ISequence<T>
        where T : struct
        where TSource : struct, ISequence<T>
    {
        TSource source;
        int count;

        public TakeSequence(ref TSource source, int count)
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
            if (source.MoveNext() && count > 0)
            {
                count -= 1;
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
            var list = source.ToList();
            if (count >= list.Length)
                return list;
            list.RemoveRangeSwapBackWithBeginEnd(count, list.Length);
            return list;
        }
    }
}
