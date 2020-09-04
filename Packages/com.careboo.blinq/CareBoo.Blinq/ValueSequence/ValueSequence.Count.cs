using System.Collections.Generic;

namespace CareBoo.Blinq
{
    public partial struct ValueSequence<T, TQuery>
        : IEnumerable<T>
        where T : struct
        where TQuery : struct, IQuery<T>
    {
        public int Count<TPredicate>(TPredicate predicate = default)
            where TPredicate : struct, IValueFunc<T, bool>
        {
            var count = 0;
            var sourceList = query.Execute();
            for (var i = 0; i < sourceList.Length; i++)
                if (predicate.Invoke(sourceList[i]))
                    count++;
            return count;
        }

        public int Count()
        {
            var sourceList = query.Execute();
            return sourceList.Length;
        }
    }
}
