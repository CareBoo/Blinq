using System;
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
            var sourceSeq = source.GetEnumerator();
            var seq = new DefaultIfEmptySequence<T, TSource>(ref sourceSeq, in defaultVal);
            return ValueSequence<T>.New(ref seq);
        }
    }

    public struct DefaultIfEmptySequence<T, TSource> : ISequence<T>
        where T : struct
        where TSource : struct, ISequence<T>
    {
        TSource source;
        readonly T defaultVal;

        int currentIndex;

        public DefaultIfEmptySequence(ref TSource source, in T defaultVal)
        {
            this.source = source;
            this.defaultVal = defaultVal;
            currentIndex = 0;
        }

        public T Current => currentIndex == 1
            ? defaultVal
            : source.Current;

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            source.Dispose();
        }

        public bool MoveNext()
        {
            if (currentIndex == 0)
            {
                if (source.MoveNext())
                    currentIndex = 2;
                else
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
            var sourceList = source.ToList();
            if (sourceList.Length == 0)
                sourceList.Add(defaultVal);
            return sourceList;
        }
    }
}
