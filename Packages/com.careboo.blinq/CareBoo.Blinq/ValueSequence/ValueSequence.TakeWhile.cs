using Unity.Collections;
using CareBoo.Burst.Delegates;
using System.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, TakeWhileIndexSequence<T, TSource, TPredicate>> TakeWhile<T, TSource, TPredicate>(
            this ValueSequence<T, TSource> source,
            ValueFunc<T, int, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TPredicate : struct, IFunc<T, int, bool>
        {
            var seq = new TakeWhileIndexSequence<T, TSource, TPredicate> { Source = source.Source, Predicate = predicate };
            return ValueSequence<T>.New(seq);
        }

        public static ValueSequence<T, TakeWhileSequence<T, TSource, TPredicate>> TakeWhile<T, TSource, TPredicate>(
            this ValueSequence<T, TSource> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var seq = new TakeWhileSequence<T, TSource, TPredicate> { Source = source.Source, Predicate = predicate };
            return ValueSequence<T>.New(seq);
        }
    }

    public struct TakeWhileIndexSequence<T, TSource, TPredicate> : ISequence<T>
        where T : struct
        where TSource : struct, ISequence<T>
        where TPredicate : struct, IFunc<T, int, bool>
    {
        public TSource Source;
        public ValueFunc<T, int, bool>.Struct<TPredicate> Predicate;

        int currentIndex;

        public T Current => Source.Current;

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            Source.Dispose();
        }

        public bool MoveNext()
        {
            if (currentIndex < 0)
                return false;
            currentIndex += 1;
            var hasNext = Source.MoveNext() && Predicate.Invoke(Source.Current, currentIndex - 1);
            if (!hasNext)
                currentIndex = -1;
            return hasNext;
        }

        public void Reset()
        {
            Source.Reset();
            currentIndex = 0;
        }

        public NativeList<T> ToList()
        {
            var list = Source.ToList();
            for (var i = 0; i < list.Length; i++)
                if (!Predicate.Invoke(list[i], i))
                {
                    list.RemoveRangeSwapBackWithBeginEnd(i, list.Length);
                    return list;
                }
            return list;
        }
    }

    public struct TakeWhileSequence<T, TSource, TPredicate> : ISequence<T>
        where T : struct
        where TSource : struct, ISequence<T>
        where TPredicate : struct, IFunc<T, bool>
    {
        public TSource Source;
        public ValueFunc<T, bool>.Struct<TPredicate> Predicate;

        bool currentIndex;

        public T Current => Source.Current;

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            Source.Dispose();
        }

        public bool MoveNext()
        {
            if (currentIndex)
                return false;
            var hasNext = Source.MoveNext() && Predicate.Invoke(Source.Current);
            currentIndex = !hasNext;
            return hasNext;
        }

        public void Reset()
        {
            Source.Reset();
            currentIndex = false;
        }

        public NativeList<T> ToList()
        {
            var list = Source.ToList();
            for (var i = 0; i < list.Length; i++)
                if (!Predicate.Invoke(list[i]))
                {
                    list.RemoveRangeSwapBackWithBeginEnd(i, list.Length);
                    return list;
                }
            return list;
        }
    }
}
