using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, SkipSequence<T, TSource, TSourceEnumerator>, SkipSequence<T, TSource, TSourceEnumerator>.Enumerator> Skip<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            int count
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var sourceSeq = source.Source;
            var seq = new SkipSequence<T, TSource, TSourceEnumerator>(in sourceSeq, count);
            return ValueSequence<T, SkipSequence<T, TSource, TSourceEnumerator>.Enumerator>.New(in seq);
        }
    }

    public struct SkipSequence<T, TSource, TSourceEnumerator>
        : ISequence<T, SkipSequence<T, TSource, TSourceEnumerator>.Enumerator>
        where T : struct
        where TSource : struct, ISequence<T, TSourceEnumerator>
        where TSourceEnumerator : struct, IEnumerator<T>
    {
        public struct Enumerator : IEnumerator<T>
        {
            TSourceEnumerator sourceEnumerator;
            int count;

            public Enumerator(
                in TSource source,
                int count
                )
            {
                sourceEnumerator = source.GetEnumerator();
                this.count = count;
            }

            public T Current => sourceEnumerator.Current;

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                sourceEnumerator.Dispose();
            }

            public bool MoveNext()
            {
                while (count > 0 && sourceEnumerator.MoveNext())
                {
                    count -= 1;
                }
                return sourceEnumerator.MoveNext();
            }

            public void Reset()
            {
                throw new NotSupportedException();
            }
        }

        readonly TSource source;
        readonly int count;

        public SkipSequence(in TSource source, int count)
        {
            this.source = source;
            this.count = count;
        }

        public NativeList<T> ToNativeList(Allocator allocator)
        {
            var list = source.ToNativeList(allocator);
            if (count < list.Length)
                list.RemoveRangeWithBeginEnd(0, count);
            else
                list.Clear();
            return list;
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(in source, count);
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
