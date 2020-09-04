using System.Collections.Generic;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public partial struct ValueSequence<T, TQuery>
        : IEnumerable<T>
        where T : struct
        where TQuery : struct, IQuery<T>
    {
        public struct WhereWithIndexQuery<TPredicate> : IQuery<T>
            where TPredicate : struct, IValueFunc<T, int, bool>
        {
            public TQuery Query;
            public TPredicate Predicate;

            public NativeList<T> Execute()
            {
                var sourceList = Query.Execute();
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

        public ValueSequence<T, WhereWithIndexQuery<TPredicate>> WhereWithIndex<TPredicate>(TPredicate predicate = default)
            where TPredicate : struct, IValueFunc<T, int, bool>
        {
            var newQuery = new WhereWithIndexQuery<TPredicate> { Query = query, Predicate = predicate };
            return new ValueSequence<T, WhereWithIndexQuery<TPredicate>>(newQuery);
        }

        public struct WhereQuery<TPredicate> : IQuery<T>
            where TPredicate : struct, IValueFunc<T, bool>
        {
            public TQuery Query;
            public TPredicate Predicate;

            public NativeList<T> Execute()
            {
                var sourceList = Query.Execute();
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

        public ValueSequence<T, WhereQuery<TPredicate>> Where<TPredicate>(TPredicate predicate = default)
            where TPredicate : struct, IValueFunc<T, bool>
        {
            var newQuery = new WhereQuery<TPredicate> { Query = query, Predicate = predicate };
            return new ValueSequence<T, WhereQuery<TPredicate>>(newQuery);
        }
    }
}
