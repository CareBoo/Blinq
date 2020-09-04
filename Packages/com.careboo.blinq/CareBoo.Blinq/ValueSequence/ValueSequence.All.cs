using System.Collections.Generic;

namespace CareBoo.Blinq
{
    public partial struct ValueSequence<T, TQuery>
        : IEnumerable<T>
        where T : struct
        where TQuery : struct, IQuery<T>
    {
        public bool All<TPredicate>(TPredicate predicate = default)
            where TPredicate : struct, IValueFunc<T, bool>
        {
            var sourceList = query.Execute();
            for (var i = 0; i < sourceList.Length; i++)
                if (!predicate.Invoke(sourceList[i]))
                    return false;
            return true;
        }
    }
}
