using System.Collections.Generic;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public partial struct ValueSequence<T, TSource>
        : IEnumerable<T>
        where T : struct
        where TSource : struct, ISequence<T>
    {
        public struct AppendQuery : ISequence<T>
        {
            public T Item;
            public TSource Query;

            public NativeList<T> Execute()
            {
                var sourceList = Query.Execute();
                sourceList.Add(Item);
                return sourceList;
            }
        }

        public ValueSequence<T, AppendQuery> Append(T item)
        {
            var newQuery = new AppendQuery { Item = item, Query = source };
            return new ValueSequence<T, AppendQuery>(newQuery);
        }
    }
}
