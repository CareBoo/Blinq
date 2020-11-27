using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<
            T,
            UnionSequence<T, TSource, TSourceEnumerator, TSecond, TSecondEnumerator>,
            UnionSequence<T, TSource, TSourceEnumerator, TSecond, TSecondEnumerator>.Enumerator>
        Union<T, TSource, TSourceEnumerator, TSecond, TSecondEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            in ValueSequence<T, TSecond, TSecondEnumerator> second
            )
            where T : unmanaged, IEquatable<T>
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TSecond : struct, ISequence<T, TSecondEnumerator>
            where TSecondEnumerator : struct, IEnumerator<T>
        {
            var sourceSeq = source.Source;
            var secondSeq = second.Source;
            var seq = new UnionSequence<T, TSource, TSourceEnumerator, TSecond, TSecondEnumerator>(in sourceSeq, in secondSeq);
            return ValueSequence<T, UnionSequence<T, TSource, TSourceEnumerator, TSecond, TSecondEnumerator>.Enumerator>.New(in seq);
        }

        public static ValueSequence<
            T,
            UnionSequence<T, TSource, TSourceEnumerator, NativeArraySequence<T>, NativeArray<T>.Enumerator>,
            UnionSequence<T, TSource, TSourceEnumerator, NativeArraySequence<T>, NativeArray<T>.Enumerator>.Enumerator>
        Union<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            in NativeArray<T> second
            )
            where T : unmanaged, IEquatable<T>
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var secondSeq = second.ToValueSequence();
            return source.Union(in secondSeq);
        }
    }

    public struct UnionSequence<T, TSource, TSourceEnumerator, TSecond, TSecondEnumerator>
        : ISequence<T, UnionSequence<T, TSource, TSourceEnumerator, TSecond, TSecondEnumerator>.Enumerator>
        where T : unmanaged, IEquatable<T>
        where TSource : struct, ISequence<T, TSourceEnumerator>
        where TSourceEnumerator : struct, IEnumerator<T>
        where TSecond : struct, ISequence<T, TSecondEnumerator>
        where TSecondEnumerator : struct, IEnumerator<T>
    {
        public struct Enumerator : IEnumerator<T>
        {
            TSourceEnumerator sourceEnumerator;
            TSecondEnumerator secondEnumerator;
            NativeHashSet<T> union;

            public Enumerator(
                in TSource source,
                in TSecond second
                )
            {
                sourceEnumerator = source.GetEnumerator();
                secondEnumerator = second.GetEnumerator();
                union = new NativeHashSet<T>(1, Allocator.Temp);
                Current = default;
            }

            public T Current { get; private set; }

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                sourceEnumerator.Dispose();
                secondEnumerator.Dispose();
                union.Dispose();
            }

            public bool MoveNext()
            {
                while (sourceEnumerator.MoveNext())
                {
                    Current = sourceEnumerator.Current;
                    if (union.Add(Current))
                        return true;
                }
                while (secondEnumerator.MoveNext())
                {
                    Current = secondEnumerator.Current;
                    if (union.Add(Current))
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

        readonly TSecond second;

        public UnionSequence(in TSource source, in TSecond second)
        {
            this.source = source;
            this.second = second;
        }

        public NativeList<T> ToNativeList(Allocator allocator)
        {
            var sourceList = source.ToNativeList(allocator);
            var secondList = second.ToNativeList(Allocator.Temp);
            var set = new NativeHashSet<T>(sourceList.Length + secondList.Length, Allocator.Temp);
            for (var i = 0; i < sourceList.Length; i++)
                set.Add(sourceList[i]);
            for (var i = 0; i < secondList.Length; i++)
                set.Add(secondList[i]);
            var setArr = set.ToNativeArray(Allocator.Temp);
            sourceList.CopyFrom(setArr);
            setArr.Dispose();
            secondList.Dispose();
            set.Dispose();
            return sourceList;
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(in source, in second);
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
