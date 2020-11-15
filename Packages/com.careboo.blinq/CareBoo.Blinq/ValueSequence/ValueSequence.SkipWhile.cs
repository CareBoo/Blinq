using Unity.Collections;
using CareBoo.Burst.Delegates;
using System.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, SkipWhileIndexSequence<T, TSource, TPredicate>> SkipWhile<T, TSource, TPredicate>(
            this in ValueSequence<T, TSource> source,
            in ValueFunc<T, int, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TPredicate : struct, IFunc<T, int, bool>
        {
            var sourceSeq = source.GetEnumerator();
            var seq = new SkipWhileIndexSequence<T, TSource, TPredicate>(ref sourceSeq, in predicate);
            return ValueSequence<T>.New(ref seq);
        }

        public static ValueSequence<T, SkipWhileSequence<T, TSource, TPredicate>> SkipWhile<T, TSource, TPredicate>(
            this in ValueSequence<T, TSource> source,
            in ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var sourceSeq = source.GetEnumerator();
            var seq = new SkipWhileSequence<T, TSource, TPredicate>(ref sourceSeq, in predicate);
            return ValueSequence<T>.New(ref seq);
        }
    }

    public struct SkipWhileIndexSequence<T, TSource, TPredicate> : ISequence<T>
        where T : struct
        where TSource : struct, ISequence<T>
        where TPredicate : struct, IFunc<T, int, bool>
    {
        TSource source;
        readonly ValueFunc<T, int, bool>.Struct<TPredicate> predicate;

        int currentIndex;

        public SkipWhileIndexSequence(ref TSource source, in ValueFunc<T, int, bool>.Struct<TPredicate> predicate)
        {
            this.source = source;
            this.predicate = predicate;
            currentIndex = 0;
        }

        public T Current => source.Current;

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            source.Dispose();
        }

        public bool MoveNext()
        {
            if (currentIndex > 0)
                return source.MoveNext();
            var isNext = source.MoveNext();
            while (isNext && predicate.Invoke(source.Current, currentIndex))
            {
                currentIndex += 1;
                isNext = source.MoveNext();
            }
            return isNext;
        }

        public void Reset()
        {
            currentIndex = default;
            source.Reset();
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
    }

    public struct SkipWhileSequence<T, TSource, TPredicate> : ISequence<T>
        where T : struct
        where TSource : struct, ISequence<T>
        where TPredicate : struct, IFunc<T, bool>
    {
        public TSource source;
        public ValueFunc<T, bool>.Struct<TPredicate> predicate;

        bool currentIndex;

        public SkipWhileSequence(ref TSource source, in ValueFunc<T, bool>.Struct<TPredicate> predicate)
        {
            this.source = source;
            this.predicate = predicate;
            currentIndex = false;
        }

        public T Current => source.Current;

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            source.Dispose();
        }

        public bool MoveNext()
        {
            if (currentIndex)
                return source.MoveNext();
            currentIndex = true;
            var isNext = source.MoveNext();
            while (isNext && predicate.Invoke(source.Current))
                isNext = source.MoveNext();
            return isNext;
        }

        public void Reset()
        {
            currentIndex = default;
            source.Reset();
        }

        public NativeList<T> ToNativeList(Allocator allocator)
        {
            var list = source.ToNativeList(allocator);
            for (var i = 0; i < list.Length; i++)
                if (!predicate.Invoke(list[i]))
                {
                    list.RemoveRangeWithBeginEnd(0, i);
                    return list;
                }
            list.Clear();
            return list;
        }
    }
}
