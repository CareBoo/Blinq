using System.Collections.Generic;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public partial struct ValueSequence<T, TSource>
        : IEnumerable<T>
        where T : struct
        where TSource : struct, ISequence<T>
    {
        public struct WhereWithIndexQuery<TPredicate> : ISequence<T>
            where TPredicate : struct, IValueFunc<T, int, bool>
        {
            public TSource Query;
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
            var newQuery = new WhereWithIndexQuery<TPredicate> { Query = source, Predicate = predicate };
            return new ValueSequence<T, WhereWithIndexQuery<TPredicate>>(newQuery);
        }

        public struct WhereQuery<TPredicate> : ISequence<T>
            where TPredicate : struct, IValueFunc<T, bool>
        {
            public TSource Query;
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
            var newQuery = new WhereQuery<TPredicate> { Query = source, Predicate = predicate };
            return new ValueSequence<T, WhereQuery<TPredicate>>(newQuery);
        }
    }
}
