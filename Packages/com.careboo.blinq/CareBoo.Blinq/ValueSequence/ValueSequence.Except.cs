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
            ExceptSequence<T, TSource, TSourceEnumerator, TSecond, TSecondEnumerator>,
            ExceptSequence<T, TSource, TSourceEnumerator, TSecond, TSecondEnumerator>.Enumerator>
        Except<T, TSource, TSourceEnumerator, TSecond, TSecondEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            in ValueSequence<T, TSecond, TSecondEnumerator> second
            )
            where T : unmanaged, IEquatable<T>
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TSecond : struct, ISequence<T, TSecondEnumerator>
            where TSecondEnumerator : struct, IEnumerator<T>
        {
            var seq = new ExceptSequence<T, TSource, TSourceEnumerator, TSecond, TSecondEnumerator>(in source.Source, in second.Source);
            return ValueSequence<T, ExceptSequence<T, TSource, TSourceEnumerator, TSecond, TSecondEnumerator>.Enumerator>.New(in seq);
        }

        public static ValueSequence<
            T,
            ExceptSequence<T, TSource, TSourceEnumerator, NativeArraySequence<T>, NativeArray<T>.Enumerator>,
            ExceptSequence<T, TSource, TSourceEnumerator, NativeArraySequence<T>, NativeArray<T>.Enumerator>.Enumerator>
        Except<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            in NativeArray<T> second
            )
            where T : unmanaged, IEquatable<T>
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            return source.Except(second.ToValueSequence());
        }
    }

    public struct ExceptSequence<T, TSource, TSourceEnumerator, TSecond, TSecondEnumerator>
        : ISequence<T, ExceptSequence<T, TSource, TSourceEnumerator, TSecond, TSecondEnumerator>.Enumerator>
        where T : unmanaged, IEquatable<T>
        where TSource : struct, ISequence<T, TSourceEnumerator>
        where TSourceEnumerator : struct, IEnumerator<T>
        where TSecond : struct, ISequence<T, TSecondEnumerator>
        where TSecondEnumerator : struct, IEnumerator<T>
    {
        public struct Enumerator : IEnumerator<T>
        {
            TSourceEnumerator sourceEnum;
            NativeHashSet<T> set;

            public Enumerator(in TSource source, in TSecond second)
            {
                sourceEnum = source.GetEnumerator();
                var secondList = second.ToNativeList(Allocator.Temp);
                set = new NativeHashSet<T>(secondList.Length, Allocator.Temp);
                for (var i = 0; i < secondList.Length; i++)
                    set.Add(secondList[i]);
                secondList.Dispose();
            }

            public T Current => sourceEnum.Current;

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                sourceEnum.Dispose();
                set.Dispose();
            }

            public bool MoveNext()
            {
                while (sourceEnum.MoveNext())
                    if (set.Add(sourceEnum.Current))
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

        public ExceptSequence(in TSource source, in TSecond second)
        {
            this.source = source;
            this.second = second;
        }

        public NativeList<T> ToNativeList(Allocator allocator)
        {
            var sourceList = source.ToNativeList(Allocator.Temp);
            var secondList = second.ToNativeList(Allocator.Temp);
            var list = new NativeList<T>(sourceList.Length, allocator);
            var secondSet = new NativeHashSet<T>(secondList.Length, Allocator.Temp);
            for (var i = 0; i < secondList.Length; i++)
                secondSet.Add(secondList[i]);
            for (var i = 0; i < sourceList.Length; i++)
                if (secondSet.Add(sourceList[i]))
                {
                    list.AddNoResize(sourceList[i]);
                }
            sourceList.Dispose();
            secondList.Dispose();
            secondSet.Dispose();
            return list;
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
