using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, TakeWhileIndexSequence<T, TSource, TPredicate>> TakeWhile<T, TSource, TPredicate>(
            this ValueSequence<T, TSource> source,
            ValueFunc<T, int, bool>.Impl<TPredicate> predicate
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TPredicate : struct, IFunc<T, int, bool>
        {
            var seq = new TakeWhileIndexSequence<T, TSource, TPredicate> { Source = source.Source, Predicate = predicate };
            return new ValueSequence<T, TakeWhileIndexSequence<T, TSource, TPredicate>>(seq);
        }

        public static ValueSequence<T, TakeWhileSequence<T, TSource, TPredicate>> TakeWhile<T, TSource, TPredicate>(
            this ValueSequence<T, TSource> source,
            ValueFunc<T, bool>.Impl<TPredicate> predicate
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var seq = new TakeWhileSequence<T, TSource, TPredicate> { Source = source.Source, Predicate = predicate };
            return new ValueSequence<T, TakeWhileSequence<T, TSource, TPredicate>>(seq);
        }
    }

    public struct TakeWhileIndexSequence<T, TSource, TPredicate> : ISequence<T>
        where T : struct
        where TSource : struct, ISequence<T>
        where TPredicate : struct, IFunc<T, int, bool>
    {
        public TSource Source;
        public ValueFunc<T, int, bool>.Impl<TPredicate> Predicate;

        public NativeList<T> Execute()
        {
            var list = Source.Execute();
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
        public ValueFunc<T, bool>.Impl<TPredicate> Predicate;

        public NativeList<T> Execute()
        {
            var list = Source.Execute();
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
