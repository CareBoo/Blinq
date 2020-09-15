using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, WhereIndexSequence<T, TSource, TPredicate>> Where<T, TSource, TPredicate>(
            this ValueSequence<T, TSource> source,
            ValueFunc<T, int, bool>.Impl<TPredicate> predicate
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TPredicate : struct, IFunc<T, int, bool>
        {
            var seq = new WhereIndexSequence<T, TSource, TPredicate> { Source = source.Source, Predicate = predicate };
            return new ValueSequence<T, WhereIndexSequence<T, TSource, TPredicate>>(seq);
        }

        public static ValueSequence<T, WhereSequence<T, TSource, TPredicate>> Where<T, TSource, TPredicate>(
            this ValueSequence<T, TSource> source,
            ValueFunc<T, bool>.Impl<TPredicate> predicate
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var seq = new WhereSequence<T, TSource, TPredicate> { Source = source.Source, Predicate = predicate };
            return new ValueSequence<T, WhereSequence<T, TSource, TPredicate>>(seq);
        }
    }

    public struct WhereIndexSequence<T, TSource, TPredicate> : ISequence<T>
        where T : struct
        where TSource : struct, ISequence<T>
        where TPredicate : struct, IFunc<T, int, bool>
    {
        public TSource Source;
        public ValueFunc<T, int, bool>.Impl<TPredicate> Predicate;

        public NativeList<T> Execute()
        {
            var sourceList = Source.Execute();
            for (var i = 0; i < sourceList.Length; i++)
            {
                if (!Predicate.Invoke(sourceList[i], i))
                {
                    sourceList.RemoveAt(i);
                    i--;
                }
            }
            return sourceList;
        }
    }

    public struct WhereSequence<T, TSource, TPredicate> : ISequence<T>
        where T : struct
        where TSource : struct, ISequence<T>
        where TPredicate : struct, IFunc<T, bool>
    {
        public TSource Source;
        public ValueFunc<T, bool>.Impl<TPredicate> Predicate;

        public NativeList<T> Execute()
        {
            var sourceList = Source.Execute();
            for (var i = 0; i < sourceList.Length; i++)
            {
                if (!Predicate.Invoke(sourceList[i]))
                {
                    sourceList.RemoveAt(i);
                    i--;
                }
            }
            return sourceList;
        }
    }
}
