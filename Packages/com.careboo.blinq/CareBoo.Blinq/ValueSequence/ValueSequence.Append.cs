using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, AppendSequence<T, TSource>> Append<T, TSource>(
            this ref ValueSequence<T, TSource> source,
            T item
            )
            where T : struct
            where TSource : struct, ISequence<T>
        {
            var seq = new AppendSequence<T, TSource> { Source = source.Source, Item = item };
            return new ValueSequence<T, AppendSequence<T, TSource>>(seq);
        }
    }
    public struct AppendSequence<T, TSource> : ISequence<T>
        where T : struct
        where TSource : struct, ISequence<T>
    {
        public TSource Source;
        public T Item;

        public NativeList<T> Execute()
        {
            var sourceList = Source.Execute();
            sourceList.Add(Item);
            return sourceList;
        }
    }
}
