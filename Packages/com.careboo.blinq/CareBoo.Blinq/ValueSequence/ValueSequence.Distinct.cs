using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, DistinctSequence<T, TSource, TSourceEnumerator>, DistinctSequence<T, TSource, TSourceEnumerator>.Enumerator> Distinct<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source
            )
            where T : unmanaged, IEquatable<T>
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var sourceSeq = source.Source;
            var seq = new DistinctSequence<T, TSource, TSourceEnumerator>(in sourceSeq);
            return ValueSequence<T, DistinctSequence<T, TSource, TSourceEnumerator>.Enumerator>.New(in seq);
        }
    }

    public struct DistinctSequence<T, TSource, TSourceEnumerator>
        : ISequence<T, DistinctSequence<T, TSource, TSourceEnumerator>.Enumerator>
        where T : unmanaged, IEquatable<T>
        where TSource : struct, ISequence<T, TSourceEnumerator>
        where TSourceEnumerator : struct, IEnumerator<T>
    {
        public struct Enumerator : IEnumerator<T>
        {
            TSourceEnumerator sourceEnumerator;
            NativeHashSet<T> hashSet;

            public Enumerator(
                in TSource source
                )
            {
                sourceEnumerator = source.GetEnumerator();
                hashSet = new NativeHashSet<T>(0, Allocator.Temp);
            }

            public T Current => sourceEnumerator.Current;

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                sourceEnumerator.Dispose();
                hashSet.Dispose();
            }

            public bool MoveNext()
            {
                while (sourceEnumerator.MoveNext())
                    if (hashSet.Add(sourceEnumerator.Current))
                        return true;
                return false;
            }

            public void Reset()
            {
                throw new NotSupportedException();
            }
        }
        readonly TSource source;

        public DistinctSequence(in TSource source)
        {
            this.source = source;
        }

        public NativeList<T> ToNativeList(Allocator allocator)
        {
            var list = source.ToNativeList(allocator);
            var tempSet = new NativeHashSet<T>(list.Length, Allocator.Temp);
            for (var i = 0; i < list.Length; i++)
                if (!tempSet.Add(list[i]))
                {
                    list.RemoveAt(i);
                    i--;
                }
            tempSet.Dispose();
            return list;
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(in source);
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
