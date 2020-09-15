using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, SkipWhileIndexSequence<T, TSource, TPredicate>> SkipWhile<T, TSource, TPredicate>(
            this ValueSequence<T, TSource> source,
            ValueFunc<T, int, bool>.Impl<TPredicate> predicate
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TPredicate : struct, IFunc<T, int, bool>
        {
            var seq = new SkipWhileIndexSequence<T, TSource, TPredicate> { Source = source.Source, Predicate = predicate };
            return new ValueSequence<T, SkipWhileIndexSequence<T, TSource, TPredicate>>(seq);
        }

        public static ValueSequence<T, SkipWhileSequence<T, TSource, TPredicate>> SkipWhile<T, TSource, TPredicate>(
            this ValueSequence<T, TSource> source,
            ValueFunc<T, bool>.Impl<TPredicate> predicate
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var seq = new SkipWhileSequence<T, TSource, TPredicate> { Source = source.Source, Predicate = predicate };
            return new ValueSequence<T, SkipWhileSequence<T, TSource, TPredicate>>(seq);
        }
    }

    public struct SkipWhileIndexSequence<T, TSource, TPredicate> : ISequence<T>
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
        public ValueFunc<T, bool>.Impl<TPredicate> Predicate;

        public NativeList<T> Execute()
        {
            var list = Source.Execute();
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
