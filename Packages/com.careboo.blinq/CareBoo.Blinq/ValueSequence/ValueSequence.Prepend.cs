using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, PrependSequence<T, TSource, TSourceEnumerator>, PrependSequence<T, TSource, TSourceEnumerator>.Enumerator> Prepend<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            in T item
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var sourceSeq = source.Source;
            var seq = new PrependSequence<T, TSource, TSourceEnumerator>(in sourceSeq, in item);
            return ValueSequence<T, PrependSequence<T, TSource, TSourceEnumerator>.Enumerator>.New(in seq);
        }
    }

    public struct PrependSequence<T, TSource, TSourceEnumerator>
        : ISequence<T, PrependSequence<T, TSource, TSourceEnumerator>.Enumerator>
        where T : struct
        where TSource : struct, ISequence<T, TSourceEnumerator>
        where TSourceEnumerator : struct, IEnumerator<T>
    {
        public struct Enumerator : IEnumerator<T>
        {
            TSourceEnumerator sourceEnumerator;
            bool currentIndex;

            public Enumerator(
                in TSource source,
                T item
                )
            {
                sourceEnumerator = source.GetEnumerator();
                Current = item;
                currentIndex = false;
            }

            public T Current { get; private set; }

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                sourceEnumerator.Dispose();
            }

            public bool MoveNext()
            {
                if (!currentIndex)
                {
                    currentIndex = true;
                    return true;
                }
                if (sourceEnumerator.MoveNext())
                {
                    Current = sourceEnumerator.Current;
                    return true;
                }
                return false;
            }

            public void Reset()
            {
                throw new NotSupportedException();
            }
        }

        readonly TSource source;

        readonly T item;

        public PrependSequence(in TSource source, in T item)
        {
            this.source = source;
            this.item = item;
        }

        public NativeList<T> ToNativeList(Allocator allocator)
        {
            var sourceList = source.ToNativeList(Allocator.Temp);
            var list = new NativeList<T>(sourceList.Length + 1, allocator);
            list.AddNoResize(item);
            list.AddRangeNoResize(sourceList);
            sourceList.Dispose();
            return list;
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(in source, item);
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
