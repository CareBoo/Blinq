using System;
using System.Collections;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, PrependSequence<T, TSource>> Prepend<T, TSource>(
            this in ValueSequence<T, TSource> source,
            in T item
            )
            where T : struct
            where TSource : struct, ISequence<T>
        {
            var sourceSeq = source.GetEnumerator();
            var seq = new PrependSequence<T, TSource>(ref sourceSeq, in item);
            return ValueSequence<T>.New(ref seq);
        }
    }

    public struct PrependSequence<T, TSource> : ISequence<T>
        where T : struct
        where TSource : struct, ISequence<T>
    {
        TSource source;
        readonly T item;
        int currentIndex;

        public PrependSequence(ref TSource source, in T item)
        {
            this.source = source;
            this.item = item;
            currentIndex = 0;
        }

        public T Current => currentIndex > 1
            ? source.Current
            : item;

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            source.Dispose();
        }

        public bool MoveNext()
        {
            if (currentIndex == 0)
            {
                currentIndex = 1;
                return true;
            }
            else if (currentIndex == 1)
                currentIndex = 2;
            return source.MoveNext();
        }

        public void Reset()
        {
            throw new NotSupportedException();
        }

        public NativeList<T> ToList()
        {
            using (var sourceList = source.ToList())
            {
                var list = new NativeList<T>(sourceList.Length + 1, Allocator.Temp);
                list.AddNoResize(item);
                list.AddRangeNoResize(sourceList);
                return list;
            }
        }
    }
}
