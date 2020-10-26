using Unity.Collections;
using CareBoo.Burst.Delegates;
using System.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, SkipWhileIndexSequence<T, TSource, TPredicate>> SkipWhile<T, TSource, TPredicate>(
            this ValueSequence<T, TSource> source,
            ValueFunc<T, int, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TPredicate : struct, IFunc<T, int, bool>
        {
            var seq = new SkipWhileIndexSequence<T, TSource, TPredicate> { Source = source.Source, Predicate = predicate };
            return ValueSequence<T>.New(seq);
        }

        public static ValueSequence<T, SkipWhileSequence<T, TSource, TPredicate>> SkipWhile<T, TSource, TPredicate>(
            this ValueSequence<T, TSource> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var seq = new SkipWhileSequence<T, TSource, TPredicate> { Source = source.Source, Predicate = predicate };
            return ValueSequence<T>.New(seq);
        }
    }

    public struct SkipWhileIndexSequence<T, TSource, TPredicate> : ISequence<T>
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
            if (currentIndex > 0)
                return Source.MoveNext();
            var isNext = Source.MoveNext();
            while (isNext && Predicate.Invoke(Source.Current, currentIndex))
            {
                currentIndex += 1;
                isNext = Source.MoveNext();
            }
            return isNext;
        }

        public void Reset()
        {
            currentIndex = default;
            Source.Reset();
        }

        public NativeList<T> ToList()
        {
            var list = Source.ToList();
            for (var i = 0; i < list.Length; i++)
                if (!Predicate.Invoke(list[i], i))
                {
                    list.RemoveRangeWithBeginEnd(0, i);
                    return list;
                }
            list.Clear();
            return list;
        }
    }

    public struct SkipWhileSequence<T, TSource, TPredicate> : ISequence<T>
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
                return Source.MoveNext();
            currentIndex = true;
            var isNext = Source.MoveNext();
            while (isNext && Predicate.Invoke(Source.Current))
                isNext = Source.MoveNext();
            return isNext;
        }

        public void Reset()
        {
            currentIndex = default;
            Source.Reset();
        }

        public NativeList<T> ToList()
        {
            var list = Source.ToList();
            for (var i = 0; i < list.Length; i++)
                if (!Predicate.Invoke(list[i]))
                {
                    list.RemoveRangeWithBeginEnd(0, i);
                    return list;
                }
            list.Clear();
            return list;
        }
    }
}
