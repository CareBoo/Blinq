using System.Collections;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, PrependSequence<T, TSource>> Prepend<T, TSource>(
            this ValueSequence<T, TSource> source,
            T item
            )
            where T : struct
            where TSource : struct, ISequence<T>
        {
            var seq = new PrependSequence<T, TSource> { Source = source.Source, Item = item };
            return ValueSequence<T>.New(seq);
        }
    }

    public struct PrependSequence<T, TSource> : ISequence<T>
        where T : struct
        where TSource : struct, ISequence<T>
    {
        public TSource Source;
        public T Item;

        byte currentIndex;

        public T Current => currentIndex > 1
            ? Source.Current
            : Item;

        object IEnumerator.Current => throw new System.NotImplementedException();

        public void Dispose()
        {
            Source.Dispose();
        }

        public bool MoveNext()
        {
            if (currentIndex > 1)
                return Source.MoveNext();
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
            using (var sourceList = Source.ToList())
            {
                var list = new NativeList<T>(sourceList.Length + 1, Allocator.Temp);
                list.AddNoResize(Item);
                list.AddRangeNoResize(sourceList);
                return list;
            }
        }
    }
}
