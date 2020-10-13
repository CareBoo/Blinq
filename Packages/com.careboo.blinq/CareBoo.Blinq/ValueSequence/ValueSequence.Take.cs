using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, TakeSequence<T, TSource>> Take<T, TSource>(
            this ValueSequence<T, TSource> source,
            int count
            )
            where T : struct
            where TSource : struct, ISequence<T>
        {
            var seq = new TakeSequence<T, TSource> { Source = source.Source, Count = count };
            return ValueSequence<T>.New(seq);
        }
    }

    public struct TakeSequence<T, TSource> : ISequence<T>
        where T : struct
        where TSource : struct, ISequence<T>
    {
        public TSource Source;
        public int Count;

        public NativeList<T> Execute()
        {
            var list = Source.Execute();
            if (Count >= list.Length)
                return list;
            list.RemoveRangeSwapBackWithBeginEnd(Count, list.Length);
            return list;
        }
    }
}
