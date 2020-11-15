using Unity.Collections;
using CareBoo.Burst.Delegates;
using System.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, TakeWhileIndexSequence<T, TSource, TPredicate>> TakeWhile<T, TSource, TPredicate>(
            this in ValueSequence<T, TSource> source,
            in ValueFunc<T, int, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TPredicate : struct, IFunc<T, int, bool>
        {
            var sourceSeq = source.GetEnumerator();
            var seq = new TakeWhileIndexSequence<T, TSource, TPredicate>(ref sourceSeq, in predicate);
            return ValueSequence<T>.New(ref seq);
        }

        public static ValueSequence<T, TakeWhileSequence<T, TSource, TPredicate>> TakeWhile<T, TSource, TPredicate>(
            this in ValueSequence<T, TSource> source,
            in ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var sourceSeq = source.GetEnumerator();
            var seq = new TakeWhileSequence<T, TSource, TPredicate>(ref sourceSeq, in predicate);
            return ValueSequence<T>.New(ref seq);
        }
    }

    public struct TakeWhileIndexSequence<T, TSource, TPredicate> : ISequence<T>
        where T : struct
        where TSource : struct, ISequence<T>
        where TPredicate : struct, IFunc<T, int, bool>
    {
        TSource source;
        readonly ValueFunc<T, int, bool>.Struct<TPredicate> predicate;

        int currentIndex;

        public TakeWhileIndexSequence(
            ref TSource source,
            in ValueFunc<T, int, bool>.Struct<TPredicate> predicate
            )
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
            if (currentIndex < -1)
                return false;
            currentIndex += 1;
            var hasNext = source.MoveNext() && predicate.Invoke(source.Current, currentIndex);
            if (!hasNext)
                currentIndex = -2;
            return hasNext;
        }

        public void Reset()
        {
            source.Reset();
            currentIndex = 0;
        }

        public NativeList<T> ToNativeList(Allocator allocator)
        {
            var list = source.ToNativeList(allocator);
            for (var i = 0; i < list.Length; i++)
                if (!predicate.Invoke(list[i], i))
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
        TSource source;
        readonly ValueFunc<T, bool>.Struct<TPredicate> predicate;

        bool currentIndex;

        public TakeWhileSequence(
            ref TSource source,
            in ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
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
                return false;
            var hasNext = source.MoveNext() && predicate.Invoke(source.Current);
            currentIndex = !hasNext;
            return hasNext;
        }

        public void Reset()
        {
            source.Reset();
            currentIndex = false;
        }

        public NativeList<T> ToNativeList(Allocator allocator)
        {
            var list = source.ToNativeList(allocator);
            for (var i = 0; i < list.Length; i++)
                if (!predicate.Invoke(list[i]))
                {
                    list.RemoveRangeSwapBackWithBeginEnd(i, list.Length);
                    return list;
                }
            return list;
        }
    }
}
