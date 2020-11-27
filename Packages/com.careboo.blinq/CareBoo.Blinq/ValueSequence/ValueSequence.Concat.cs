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
            ConcatSequence<T, TSource, TSourceEnumerator, TSecond, TSecondEnumerator>,
            ConcatSequence<T, TSource, TSourceEnumerator, TSecond, TSecondEnumerator>.Enumerator>
        Concat<T, TSource, TSourceEnumerator, TSecond, TSecondEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueSequence<T, TSecond, TSecondEnumerator> second
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TSecond : struct, ISequence<T, TSecondEnumerator>
            where TSecondEnumerator : struct, IEnumerator<T>
        {
            var sourceSeq = source.Source;
            var secondSeq = second.Source;
            var newSequence = new ConcatSequence<T, TSource, TSourceEnumerator, TSecond, TSecondEnumerator>(in sourceSeq, in secondSeq);
            return ValueSequence<T, ConcatSequence<T, TSource, TSourceEnumerator, TSecond, TSecondEnumerator>.Enumerator>.New(in newSequence);
        }

        public static ValueSequence<
            T,
            ConcatSequence<T, TSource, TSourceEnumerator, NativeArraySequence<T>, NativeArray<T>.Enumerator>,
            ConcatSequence<T, TSource, TSourceEnumerator, NativeArraySequence<T>, NativeArray<T>.Enumerator>.Enumerator>
        Concat<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            NativeArray<T> second
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            return source.Concat(second.ToValueSequence());
        }
    }

    public struct ConcatSequence<T, TSource, TSourceEnumerator, TSecond, TSecondEnumerator>
        : ISequence<T, ConcatSequence<T, TSource, TSourceEnumerator, TSecond, TSecondEnumerator>.Enumerator>
        where T : struct
        where TSource : struct, ISequence<T, TSourceEnumerator>
        where TSourceEnumerator : struct, IEnumerator<T>
        where TSecond : struct, ISequence<T, TSecondEnumerator>
        where TSecondEnumerator : struct, IEnumerator<T>
    {
        public struct Enumerator : IEnumerator<T>
        {
            TSourceEnumerator sourceEnumerator;
            TSecondEnumerator secondEnumerator;
            bool sourceHasCurrent;

            public Enumerator(in TSource source, in TSecond second)
            {
                sourceEnumerator = source.GetEnumerator();
                secondEnumerator = second.GetEnumerator();
                sourceHasCurrent = true;
            }

            public T Current => sourceHasCurrent
                ? sourceEnumerator.Current
                : secondEnumerator.Current;

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                sourceEnumerator.Dispose();
                secondEnumerator.Dispose();
            }

            public bool MoveNext()
            {
                if (sourceHasCurrent && sourceEnumerator.MoveNext())
                {
                    return true;
                }
                sourceHasCurrent = false;
                return secondEnumerator.MoveNext();
            }

            public void Reset()
            {
                throw new NotSupportedException();
            }
        }

        readonly TSource source;

        readonly TSecond second;

        public ConcatSequence(in TSource source, in TSecond second)
        {
            this.source = source;
            this.second = second;
        }

        public NativeList<T> ToNativeList(Allocator allocator)
        {
            var sourceList = source.ToNativeList(allocator);
            var secondList = second.ToNativeList(Allocator.Temp);
            sourceList.AddRange(secondList);
            secondList.Dispose();
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
