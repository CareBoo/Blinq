using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, TakeSequence<T, TSource, TSourceEnumerator>, TakeSequence<T, TSource, TSourceEnumerator>.Enumerator> Take<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            int count
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var sourceSeq = source.Source;
            var seq = new TakeSequence<T, TSource, TSourceEnumerator>(in sourceSeq, count);
            return ValueSequence<T, TakeSequence<T, TSource, TSourceEnumerator>.Enumerator>.New(in seq);
        }
    }

    public struct TakeSequence<T, TSource, TSourceEnumerator>
        : ISequence<T, TakeSequence<T, TSource, TSourceEnumerator>.Enumerator>
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
                count -= 1;
                return count >= 0 && sourceEnumerator.MoveNext();
            }

            public void Reset()
            {
                throw new NotSupportedException();
            }
        }

        readonly TSource source;

        readonly int count;

        public TakeSequence(in TSource source, int count)
        {
            this.source = source;
            this.count = count;
        }

        public NativeList<T> ToNativeList(Allocator allocator)
        {
            var list = source.ToNativeList(allocator);
            if (count >= list.Length)
                return list;
            list.RemoveRangeSwapBackWithBeginEnd(count, list.Length);
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
