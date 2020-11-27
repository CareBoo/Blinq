using Unity.Collections;
using CareBoo.Burst.Delegates;
using System.Collections;
using System;
using System.Collections.Generic;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<
            T,
            WhereSequence<T, TSource, TSourceEnumerator, TPredicate>,
            WhereSequence<T, TSource, TSourceEnumerator, TPredicate>.Enumerator>
        Where<T, TSource, TSourceEnumerator, TPredicate>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, int, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
        where TSourceEnumerator : struct, IEnumerator<T>
            where TPredicate : struct, IFunc<T, int, bool>
        {
            var sourceSeq = source.Source;
            var seq = new WhereSequence<T, TSource, TSourceEnumerator, TPredicate>(in sourceSeq, predicate);
            return ValueSequence<T, WhereSequence<T, TSource, TSourceEnumerator, TPredicate>.Enumerator>.New(in seq);
        }

        public static ValueSequence<
            T,
            WhereSequence<T, TSource, TSourceEnumerator, IgnoreIndex<T, bool, TPredicate>>,
            WhereSequence<T, TSource, TSourceEnumerator, IgnoreIndex<T, bool, TPredicate>>.Enumerator>
        Where<T, TSource, TSourceEnumerator, TPredicate>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
        where TSourceEnumerator : struct, IEnumerator<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var indexPredicate = UtilFunctions.IgnoreIndex(predicate);
            return source.Where(indexPredicate);
        }
    }

    public struct WhereSequence<T, TSource, TSourceEnumerator, TPredicate>
        : ISequence<T, WhereSequence<T, TSource, TSourceEnumerator, TPredicate>.Enumerator>
        where T : struct
        where TSource : struct, ISequence<T, TSourceEnumerator>
        where TSourceEnumerator : struct, IEnumerator<T>
        where TPredicate : struct, IFunc<T, int, bool>
    {
        public struct Enumerator : IEnumerator<T>
        {
            TSourceEnumerator sourceEnumerator;
            readonly ValueFunc<T, int, bool>.Struct<TPredicate> predicate;
            int currentIndex;

            public Enumerator(
                in TSource source,
                ValueFunc<T, int, bool>.Struct<TPredicate> predicate
                )
            {
                sourceEnumerator = source.GetEnumerator();
                this.predicate = predicate;
                currentIndex = -1;
            }


            public T Current => sourceEnumerator.Current;

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                sourceEnumerator.Dispose();
            }

            public bool MoveNext()
            {
                while (sourceEnumerator.MoveNext())
                {
                    currentIndex += 1;
                    if (predicate.Invoke(Current, currentIndex))
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

        readonly ValueFunc<T, int, bool>.Struct<TPredicate> predicate;

        public WhereSequence(in TSource source, ValueFunc<T, int, bool>.Struct<TPredicate> predicate)
        {
            this.source = source;
            this.predicate = predicate;
        }

        public NativeList<T> ToNativeList(Allocator allocator)
        {
            var sourceList = source.ToNativeList(allocator);
            for (var i = 0; i < sourceList.Length; i++)
                if (!predicate.Invoke(sourceList[i], i))
                {
                    sourceList.RemoveAt(i);
                    i--;
                }
            return sourceList;
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(in source, predicate);
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
