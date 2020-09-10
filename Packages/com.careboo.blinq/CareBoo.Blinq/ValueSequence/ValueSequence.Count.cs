using System.Collections.Generic;

namespace CareBoo.Blinq
{
    public partial struct ValueSequence<T, TSource>
        : IEnumerable<T>
        where T : struct
        where TSource : struct, ISequence<T>
    {
        public int Count<TPredicate>(TPredicate predicate = default)
            where TPredicate : struct, IValueFunc<T, bool>
        {
            var count = 0;
            var sourceList = source.Execute();
            for (var i = 0; i < sourceList.Length; i++)
                if (predicate.Invoke(sourceList[i]))
                    count++;
            return count;
        }

        public int Count()
        {
            var sourceList = source.Execute();
            return sourceList.Length;
        }
    }
}
