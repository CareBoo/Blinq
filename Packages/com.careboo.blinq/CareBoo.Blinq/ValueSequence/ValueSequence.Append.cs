using System.Collections.Generic;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public partial struct ValueSequence<T, TQuery>
        : IEnumerable<T>
        where T : struct
        where TQuery : struct, IQuery<T>
    {
        public struct AppendQuery : IQuery<T>
        {
            public T Item;
            public TQuery Query;

            public NativeList<T> Execute()
            {
                var sourceList = Query.Execute();
                sourceList.Add(Item);
                return sourceList;
            }
        }

        public ValueSequence<T, AppendQuery> Append(T item)
        {
            var newQuery = new AppendQuery { Item = item, Query = query };
            return new ValueSequence<T, AppendQuery>(newQuery);
        }
    }
}
