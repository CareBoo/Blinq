using System.Collections;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, AppendSequence<T, TSource>> Append<T, TSource>(
            this ValueSequence<T, TSource> source,
            T item
            )
            where T : struct
            where TSource : struct, ISequence<T>
        {
            var seq = new AppendSequence<T, TSource> { Source = source.Source, Item = item };
            return ValueSequence<T>.New(seq);
        }
    }

    public struct AppendSequence<T, TSource> : ISequence<T>
        where T : struct
        where TSource : struct, ISequence<T>
    {
        public TSource Source;
        public T Item;

        byte currentIndex;

        public T Current => currentIndex > 0
            ? Item
            : Source.Current;

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            Source.Dispose();
        }

        public bool MoveNext()
        {
            if (currentIndex > 1)
                return false;
            if (Source.MoveNext())
                return true;
            currentIndex += 1;
            return true;
        }

        public void Reset()
        {
            Source.Reset();
            currentIndex = 0;
        }

        public NativeList<T> ToList()
        {
            var sourceList = Source.ToList();
            sourceList.Add(Item);
            return sourceList;
        }
    }
}
