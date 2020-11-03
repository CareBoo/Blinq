using Unity.Collections;
using CareBoo.Burst.Delegates;
using System.Collections;
using System;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, WhereIndexSequence<T, TSource, TPredicate>> Where<T, TSource, TPredicate>(
            this in ValueSequence<T, TSource> source,
            in ValueFunc<T, int, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TPredicate : struct, IFunc<T, int, bool>
        {
            var sourceSeq = source.GetEnumerator();
            var seq = new WhereIndexSequence<T, TSource, TPredicate>(ref sourceSeq, in predicate);
            return ValueSequence<T>.New(ref seq);
        }

        public static ValueSequence<T, WhereSequence<T, TSource, TPredicate>> Where<T, TSource, TPredicate>(
            this in ValueSequence<T, TSource> source,
            in ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var sourceSeq = source.GetEnumerator();
            var seq = new WhereSequence<T, TSource, TPredicate>(ref sourceSeq, in predicate);
            return ValueSequence<T>.New(ref seq);
        }
    }

    public struct WhereIndexSequence<T, TSource, TPredicate> : ISequence<T>
        where T : struct
        where TSource : struct, ISequence<T>
        where TPredicate : struct, IFunc<T, int, bool>
    {
        TSource source;
        readonly ValueFunc<T, int, bool>.Struct<TPredicate> predicate;

        int currentIndex;

        public WhereIndexSequence(ref TSource source, in ValueFunc<T, int, bool>.Struct<TPredicate> predicate)
        {
            this.source = source;
            this.predicate = predicate;
            currentIndex = -1;
        }

        public T Current => source.Current;

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            source.Dispose();
        }

        public bool MoveNext()
        {
            while (source.MoveNext())
            {
                currentIndex += 1;
                if (predicate.Invoke(source.Current, currentIndex))
                    return true;
            }
            return false;
        }

        public void Reset()
        {
            throw new NotSupportedException();
        }

        public NativeList<T> ToList()
        {
            var sourceList = source.ToList();
            for (var i = 0; i < sourceList.Length; i++)
                if (!predicate.Invoke(sourceList[i], i))
                {
                    sourceList.RemoveAt(i);
                    i--;
                }
            return sourceList;
        }
    }

    public struct WhereSequence<T, TSource, TPredicate> : ISequence<T>
        where T : struct
        where TSource : struct, ISequence<T>
        where TPredicate : struct, IFunc<T, bool>
    {
        TSource source;
        readonly ValueFunc<T, bool>.Struct<TPredicate> predicate;

        public WhereSequence(ref TSource source, in ValueFunc<T, bool>.Struct<TPredicate> predicate)
        {
            this.source = source;
            this.predicate = predicate;
        }

        public T Current => source.Current;

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            source.Dispose();
        }

        public bool MoveNext()
        {
            while (source.MoveNext())
                if (predicate.Invoke(source.Current))
                    return true;
            return false;
        }

        public void Reset()
        {
            throw new NotSupportedException();
        }

        public NativeList<T> ToList()
        {
            var sourceList = source.ToList();
            for (var i = 0; i < sourceList.Length; i++)
                if (!predicate.Invoke(sourceList[i]))
                {
                    sourceList.RemoveAt(i);
                    i--;
                }
            return sourceList;
        }
    }
}
