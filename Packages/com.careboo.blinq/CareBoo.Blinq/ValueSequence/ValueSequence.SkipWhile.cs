using Unity.Collections;
using CareBoo.Burst.Delegates;
using System.Collections;
using System.Collections.Generic;
using System;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<
            T,
            SkipWhileSequence<T, TSource, TSourceEnumerator, TPredicate>,
            SkipWhileSequence<T, TSource, TSourceEnumerator, TPredicate>.Enumerator>
        SkipWhile<T, TSource, TSourceEnumerator, TPredicate>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, int, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TPredicate : struct, IFunc<T, int, bool>
        {
            var sourceSeq = source.Source;
            var seq = new SkipWhileSequence<T, TSource, TSourceEnumerator, TPredicate>(in sourceSeq, predicate);
            return ValueSequence<T, SkipWhileSequence<T, TSource, TSourceEnumerator, TPredicate>.Enumerator>.New(in seq);
        }

        public static ValueSequence<
            T,
            SkipWhileSequence<T, TSource, TSourceEnumerator, IgnoreIndex<T, bool, TPredicate>>,
            SkipWhileSequence<T, TSource, TSourceEnumerator, IgnoreIndex<T, bool, TPredicate>>.Enumerator>
        SkipWhile<T, TSource, TSourceEnumerator, TPredicate>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var indexPredicate = UtilFunctions.IgnoreIndex(predicate);
            return source.SkipWhile(indexPredicate);
        }
    }

    public struct SkipWhileSequence<T, TSource, TSourceEnumerator, TPredicate>
        : ISequence<T, SkipWhileSequence<T, TSource, TSourceEnumerator, TPredicate>.Enumerator>
        where T : struct
        where TSource : struct, ISequence<T, TSourceEnumerator>
        where TSourceEnumerator : struct, IEnumerator<T>
        where TPredicate : struct, IFunc<T, int, bool>
    {
        public struct Enumerator : IEnumerator<T>
        {
            readonly ValueFunc<T, int, bool>.Struct<TPredicate> predicate;
            TSourceEnumerator sourceEnumerator;
            int currentIndex;

            public Enumerator(
                in TSource source,
                ValueFunc<T, int, bool>.Struct<TPredicate> predicate
                )
            {
                this.predicate = predicate;
                sourceEnumerator = source.GetEnumerator();
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
                if (currentIndex > -1)
                    return sourceEnumerator.MoveNext();
                while (sourceEnumerator.MoveNext())
                {
                    currentIndex += 1;
                    if (!predicate.Invoke(sourceEnumerator.Current, currentIndex))
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


        public SkipWhileSequence(in TSource source, ValueFunc<T, int, bool>.Struct<TPredicate> predicate)
        {
            this.source = source;
            this.predicate = predicate;
        }

        public NativeList<T> ToNativeList(Allocator allocator)
        {
            var list = source.ToNativeList(allocator);
            for (var i = 0; i < list.Length; i++)
                if (!predicate.Invoke(list[i], i))
                {
                    list.RemoveRangeWithBeginEnd(0, i);
                    return list;
                }
            list.Clear();
            return list;
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
