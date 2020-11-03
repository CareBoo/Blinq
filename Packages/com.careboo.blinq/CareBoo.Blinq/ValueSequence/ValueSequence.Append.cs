using System;
using System.Collections;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, AppendSequence<T, TSource>> Append<T, TSource>(
            this in ValueSequence<T, TSource> source,
            in T item
            )
            where T : struct
            where TSource : struct, ISequence<T>
        {
            var sourceSeq = source.GetEnumerator();
            var seq = new AppendSequence<T, TSource>(ref sourceSeq, in item);
            return ValueSequence<T>.New(ref seq);
        }
    }

    public struct AppendSequence<T, TSource> : ISequence<T>
        where T : struct
        where TSource : struct, ISequence<T>
    {
        TSource source;
        readonly T item;

        int currentIndex;

        public AppendSequence(ref TSource source, in T item)
        {
            this.source = source;
            this.item = item;
            currentIndex = 0;
        }

        public T Current => currentIndex > 0
            ? item
            : source.Current;

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            source.Dispose();
        }

        public bool MoveNext()
        {
            if (currentIndex > 1)
                return false;
            if (source.MoveNext())
                return true;
            if (currentIndex < 2)
                currentIndex += 1;
            return true;
        }

        public void Reset()
        {
            throw new NotSupportedException();
        }

        public NativeList<T> ToList()
        {
            var sourceList = source.ToList();
            sourceList.Add(item);
            return sourceList;
        }
    }
}
