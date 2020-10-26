using System.Collections;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, ReverseSequence<T, TSource>> Reverse<T, TSource>(
            this ValueSequence<T, TSource> source
            )
            where T : struct
            where TSource : struct, ISequence<T>
        {
            var seq = new ReverseSequence<T, TSource> { Source = source.Source };
            return ValueSequence<T>.New(seq);
        }
    }

    public struct ReverseSequence<T, TSource> : ISequence<T>
        where T : struct
        where TSource : struct, ISequence<T>
    {
        public TSource Source;

        int currentIndex;
        NativeList<T> list;

        public T Current => list[currentIndex];

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            Source.Dispose();
            if (list.IsCreated)
                list.Dispose();
        }

        public bool MoveNext()
        {
            if (!list.IsCreated)
                list = ToList();
            else
                currentIndex += 1;
            return currentIndex < list.Length;
        }

        public void Reset()
        {
            Source.Reset();
            if (list.IsCreated)
                list.Dispose();
            list = default;
            currentIndex = default;
        }

        public NativeList<T> ToList()
        {
            var list = Source.ToList();
            for (var i = 0; i < list.Length / 2; i++)
            {
                var swap = list.Length - 1 - i;
                var tmp = list[i];
                list[i] = list[swap];
                list[swap] = tmp;
            }
            return list;
        }
    }
}
