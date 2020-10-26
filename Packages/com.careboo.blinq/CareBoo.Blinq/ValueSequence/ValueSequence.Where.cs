using Unity.Collections;
using CareBoo.Burst.Delegates;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, WhereIndexSequence<T, TSource, TPredicate>> Where<T, TSource, TPredicate>(
            this ValueSequence<T, TSource> source,
            ValueFunc<T, int, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TPredicate : struct, IFunc<T, int, bool>
        {
            var seq = new WhereIndexSequence<T, TSource, TPredicate> { Source = source.Source, Predicate = predicate };
            return ValueSequence<T>.New(seq);
        }

        public static ValueSequence<T, WhereSequence<T, TSource, TPredicate>> Where<T, TSource, TPredicate>(
            this ValueSequence<T, TSource> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var seq = new WhereSequence<T, TSource, TPredicate> { Source = source.Source, Predicate = predicate };
            return ValueSequence<T>.New(seq);
        }
    }

    public struct WhereIndexSequence<T, TSource, TPredicate> : ISequence<T>
        where T : struct
        where TSource : struct, ISequence<T>
        where TPredicate : struct, IFunc<T, int, bool>
    {
        public TSource Source;
        public ValueFunc<T, int, bool>.Struct<TPredicate> Predicate;

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
        public ValueFunc<T, bool>.Struct<TPredicate> Predicate;

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
