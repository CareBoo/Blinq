using Unity.Collections;

namespace CareBoo.Blinq
{
    public partial struct ValueSequence<T, TSource>
    {
        public struct WhereWithIndexSequence<TPredicate> : ISequence<T>
            where TPredicate : struct, IValueFunc<T, int, bool>
        {
            public TSource Source;
            public TPredicate Predicate;

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

        public ValueSequence<T, WhereWithIndexSequence<TPredicate>> WhereWithIndex<TPredicate>(TPredicate predicate = default)
            where TPredicate : struct, IValueFunc<T, int, bool>
        {
            var newSequence = new WhereWithIndexSequence<TPredicate> { Source = source, Predicate = predicate };
            return new ValueSequence<T, WhereWithIndexSequence<TPredicate>>(newSequence);
        }

        public struct WhereSequence<TPredicate> : ISequence<T>
            where TPredicate : struct, IValueFunc<T, bool>
        {
            public TSource Source;
            public TPredicate Predicate;

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

        public ValueSequence<T, WhereSequence<TPredicate>> Where<TPredicate>(TPredicate predicate = default)
            where TPredicate : struct, IValueFunc<T, bool>
        {
            var newSequence = new WhereSequence<TPredicate> { Source = source, Predicate = predicate };
            return new ValueSequence<T, WhereSequence<TPredicate>>(newSequence);
        }
    }
}
