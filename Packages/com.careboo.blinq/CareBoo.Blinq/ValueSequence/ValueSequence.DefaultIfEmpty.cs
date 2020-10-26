using System.Collections;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, DefaultIfEmptySequence<T, TSource>> DefaultIfEmpty<T, TSource>(
            this ValueSequence<T, TSource> source,
            T defaultVal = default
            )
            where T : struct
            where TSource : struct, ISequence<T>
        {
            var seq = new DefaultIfEmptySequence<T, TSource> { Source = source.Source, Default = defaultVal };
            return ValueSequence<T>.New(seq);
        }
    }

    public struct DefaultIfEmptySequence<T, TSource> : ISequence<T>
        where T : struct
        where TSource : struct, ISequence<T>
    {
        public TSource Source;
        public T Default;

        bool currentIndex;

        public T Current => currentIndex
            ? Default
            : Source;

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            Source.Dispose();
        }

        public bool MoveNext()
        {
            if (Source.MoveNext())
                return true;
            if (currentIndex)
                return false;
            currentIndex = true;
            return true;
        }

        public void Reset()
        {
            Source.Reset();
            currentIndex = false;
        }

        public NativeList<T> ToList()
        {
            var sourceList = Source.ToList();
            if (sourceList.Length == 0)
                sourceList.Add(Default);
            return sourceList;
        }
    }
}
