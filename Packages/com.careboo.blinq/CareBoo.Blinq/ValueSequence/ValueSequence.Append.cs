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

        bool currentIndex;

        public AppendSequence(ref TSource source, in T item)
        {
            this.source = source;
            this.item = item;
            currentIndex = false;
        }

        public T Current => currentIndex
            ? item
            : source.Current;

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            source.Dispose();
        }

        public bool MoveNext()
        {
            if (currentIndex)
                return false;
            if (source.MoveNext())
                return true;
            currentIndex = true;
            return true;
        }

        public void Reset()
        {
            throw new NotSupportedException();
        }

        public NativeList<T> ToNativeList(Allocator allocator)
        {
            var sourceList = source.ToNativeList(allocator);
            sourceList.Add(item);
            return sourceList;
        }
    }
}
