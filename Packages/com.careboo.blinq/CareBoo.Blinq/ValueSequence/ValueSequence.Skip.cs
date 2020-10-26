using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, SkipSequence<T, TSource>> Skip<T, TSource>(
            this ValueSequence<T, TSource> source,
            int count
            )
            where T : struct
            where TSource : struct, ISequence<T>
        {
            var seq = new SkipSequence<T, TSource> { Source = source.Source, Count = count };
            return ValueSequence<T>.New(seq);
        }
    }

    public struct SkipSequence<T, TSource> : ISequence<T>
        where T : struct
        where TSource : struct, ISequence<T>
    {
        public TSource Source;
        public int Count;

        public NativeList<T> ToList()
        {
            var list = Source.ToList();
            if (Count < list.Length)
                list.RemoveRangeWithBeginEnd(0, Count);
            else
                list.Clear();
            return list;
        }
    }
}
