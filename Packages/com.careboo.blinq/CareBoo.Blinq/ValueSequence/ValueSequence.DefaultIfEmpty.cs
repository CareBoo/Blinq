using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, DefaultIfEmptySequence<T, TSource>> DefaultIfEmpty<T, TSource>(
            this ref ValueSequence<T, TSource> source,
            T defaultVal = default
            )
            where T : struct
            where TSource : struct, ISequence<T>
        {
            var seq = new DefaultIfEmptySequence<T, TSource> { Source = source.Source, Default = defaultVal };
            return new ValueSequence<T, DefaultIfEmptySequence<T, TSource>>(seq);
        }
    }

    public struct DefaultIfEmptySequence<T, TSource> : ISequence<T>
        where T : struct
        where TSource : struct, ISequence<T>
    {
        public TSource Source;

        public T Default;

        public NativeList<T> Execute()
        {
            var sourceList = Source.Execute();
            if (sourceList.Length == 0)
                sourceList.Add(Default);
            return sourceList;
        }
    }
}
