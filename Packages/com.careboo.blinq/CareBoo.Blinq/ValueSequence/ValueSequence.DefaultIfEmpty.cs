using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, DefaultIfEmptySequence<T, TSource, TSourceEnumerator>, DefaultIfEmptySequence<T, TSource, TSourceEnumerator>.Enumerator> DefaultIfEmpty<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            in T defaultVal = default
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
        where TSourceEnumerator : struct, IEnumerator<T>
        {
            var sourceSeq = source.Source;
            var seq = new DefaultIfEmptySequence<T, TSource, TSourceEnumerator>(in sourceSeq, in defaultVal);
            return ValueSequence<T, DefaultIfEmptySequence<T, TSource, TSourceEnumerator>.Enumerator>.New(in seq);
        }
    }

    public struct DefaultIfEmptySequence<T, TSource, TSourceEnumerator>
        : ISequence<T, DefaultIfEmptySequence<T, TSource, TSourceEnumerator>.Enumerator>
        where T : struct
        where TSource : struct, ISequence<T, TSourceEnumerator>
        where TSourceEnumerator : struct, IEnumerator<T>
    {
        public struct Enumerator : IEnumerator<T>
        {
            TSourceEnumerator sourceEnumerator;
            readonly T defaultVal;
            int currentIndex;

            public Enumerator(
                in TSource source,
                in T defaultVal
                )
            {
                sourceEnumerator = source.GetEnumerator();
                this.defaultVal = defaultVal;
                currentIndex = -1;
            }

            public T Current => currentIndex == 0
                ? defaultVal
                : sourceEnumerator.Current;

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                sourceEnumerator.Dispose();
            }

            public bool MoveNext()
            {
                if (currentIndex < 0)
                {
                    if (sourceEnumerator.MoveNext())
                        currentIndex = 1;
                    else
                        currentIndex = 0;
                    return true;
                }
                else if (currentIndex == 0)
                    currentIndex = 1;
                return sourceEnumerator.MoveNext();
            }

            public void Reset()
            {
                throw new NotSupportedException();
            }
        }

        readonly TSource source;
        readonly T defaultVal;

        public DefaultIfEmptySequence(in TSource source, in T defaultVal)
        {
            this.source = source;
            this.defaultVal = defaultVal;
        }

        public NativeList<T> ToNativeList(Allocator allocator)
        {
            var sourceList = source.ToNativeList(allocator);
            if (sourceList.Length == 0)
                sourceList.Add(defaultVal);
            return sourceList;
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(in source, in defaultVal);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
