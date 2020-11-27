using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, AppendSequence<T, TSource, TSourceEnumerator>, AppendSequence<T, TSource, TSourceEnumerator>.Enumerator> Append<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            in T item
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var sourceSeq = source.Source;
            var seq = new AppendSequence<T, TSource, TSourceEnumerator>(in sourceSeq, in item);
            return ValueSequence<T, AppendSequence<T, TSource, TSourceEnumerator>.Enumerator>.New(in seq);
        }
    }

    public struct AppendSequence<T, TSource, TSourceEnumerator>
        : ISequence<T, AppendSequence<T, TSource, TSourceEnumerator>.Enumerator>
        where T : struct
        where TSource : struct, ISequence<T, TSourceEnumerator>
        where TSourceEnumerator : struct, IEnumerator<T>
    {
        public struct Enumerator : IEnumerator<T>
        {
            readonly T item;
            TSourceEnumerator sourceEnumerator;
            bool currentIndex;

            public Enumerator(
                in TSource source,
                in T item
                )
            {
                this.item = item;
                sourceEnumerator = source.GetEnumerator();
                currentIndex = false;
            }

            public T Current => currentIndex
                ? item
                : sourceEnumerator.Current;

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                sourceEnumerator.Dispose();
            }

            public bool MoveNext()
            {
                if (!sourceEnumerator.MoveNext())
                {
                    if (currentIndex)
                        return false;
                    currentIndex = true;
                }
                return true;
            }

            public void Reset()
            {
                throw new NotSupportedException();
            }
        }

        readonly TSource source;

        readonly T item;

        public AppendSequence(in TSource source, in T item)
        {
            this.source = source;
            this.item = item;
        }

        public NativeList<T> ToNativeList(Allocator allocator)
        {
            var sourceList = source.ToNativeList(allocator);
            sourceList.Add(item);
            return sourceList;
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(in source, in item);
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
