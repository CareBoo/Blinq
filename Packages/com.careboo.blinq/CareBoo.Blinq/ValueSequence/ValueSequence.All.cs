using System.Collections.Generic;

namespace CareBoo.Blinq
{
    public partial struct ValueSequence<T, TSource>
        : IEnumerable<T>
        where T : struct
        where TSource : struct, ISequence<T>
    {
        public bool All<TPredicate>(TPredicate predicate = default)
            where TPredicate : struct, IValueFunc<T, bool>
        {
            var sourceList = source.Execute();
            for (var i = 0; i < sourceList.Length; i++)
                if (!predicate.Invoke(sourceList[i]))
                    return false;
            return true;
        }
    }
}
