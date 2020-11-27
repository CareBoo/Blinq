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
            IntersectSequence<T, TSource, TSourceEnumerator, TSecond, TSecondEnumerator>,
            IntersectSequence<T, TSource, TSourceEnumerator, TSecond, TSecondEnumerator>.Enumerator>
        Intersect<T, TSource, TSourceEnumerator, TSecond, TSecondEnumerator>(
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
            var seq = new IntersectSequence<T, TSource, TSourceEnumerator, TSecond, TSecondEnumerator>(in sourceSeq, in secondSeq);
            return ValueSequence<T, IntersectSequence<T, TSource, TSourceEnumerator, TSecond, TSecondEnumerator>.Enumerator>.New(in seq);
        }

        public static ValueSequence<
            T,
            IntersectSequence<T, TSource, TSourceEnumerator, NativeArraySequence<T>, NativeArray<T>.Enumerator>,
            IntersectSequence<T, TSource, TSourceEnumerator, NativeArraySequence<T>, NativeArray<T>.Enumerator>.Enumerator>
        Intersect<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            in NativeArray<T> second
            )
            where T : unmanaged, IEquatable<T>
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>

        {
            return source.Intersect(second.ToValueSequence());
        }
    }

    public struct IntersectSequence<T, TSource, TSourceEnumerator, TSecond, TSecondEnumerator>
        : ISequence<T, IntersectSequence<T, TSource, TSourceEnumerator, TSecond, TSecondEnumerator>.Enumerator>
        where T : unmanaged, IEquatable<T>
        where TSource : struct, ISequence<T, TSourceEnumerator>
        where TSourceEnumerator : struct, IEnumerator<T>
        where TSecond : struct, ISequence<T, TSecondEnumerator>
        where TSecondEnumerator : struct, IEnumerator<T>
    {
        public struct Enumerator : IEnumerator<T>
        {
            TSecondEnumerator secondEnumerator;
            NativeHashSet<T> sourceSet;
            NativeHashSet<T> secondSet;

            public Enumerator(
                in TSource source,
                in TSecond second
                )
            {
                var sourceList = source.ToNativeList(Allocator.Temp);
                sourceSet = new NativeHashSet<T>(sourceList.Length, Allocator.Temp);
                for (var i = 0; i < sourceList.Length; i++)
                    sourceSet.Add(sourceList[i]);
                sourceList.Dispose();
                secondEnumerator = second.GetEnumerator();
                secondSet = new NativeHashSet<T>(1, Allocator.Temp);
            }

            public T Current => secondEnumerator.Current;

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                sourceSet.Dispose();
                secondEnumerator.Dispose();
                secondSet.Dispose();
            }

            public bool MoveNext()
            {
                while (secondEnumerator.MoveNext())
                    if (sourceSet.Contains(Current) && secondSet.Add(Current))
                        return true;
                return false;
            }

            public void Reset()
            {
                throw new NotSupportedException();
            }
        }

        readonly TSource source;

        readonly TSecond second;

        public IntersectSequence(in TSource source, in TSecond second)
        {
            this.source = source;
            this.second = second;
        }

        public NativeList<T> ToNativeList(Allocator allocator)
        {
            var sourceList = source.ToNativeList(Allocator.Temp);
            var secondList = second.ToNativeList(Allocator.Temp);
            var sourceSet = new NativeHashSet<T>(sourceList.Length, Allocator.Temp);
            var secondSet = new NativeHashSet<T>(secondList.Length, Allocator.Temp);
            var result = new NativeList<T>(secondList.Length, allocator);
            for (var i = 0; i < sourceList.Length; i++)
                sourceSet.Add(sourceList[i]);
            for (var i = 0; i < secondList.Length; i++)
                if (sourceSet.Contains(secondList[i]) && secondSet.Add(secondList[i]))
                    result.AddNoResize(secondList[i]);
            sourceList.Dispose();
            secondList.Dispose();
            sourceSet.Dispose();
            secondSet.Dispose();
            return result;
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
