using System;

namespace CareBoo.Blinq
{
    public partial struct ValueSequence<T, TSource>
        where T : unmanaged, IEquatable<T>
        where TSource : struct, ISequence<T>
    {
        public bool Any<TPredicate>(TPredicate predicate = default)
            where TPredicate : struct, IValueFunc<T, bool>
        {
            var sourceList = source.Execute();
            for (var i = 0; i < sourceList.Length; i++)
                if (predicate.Invoke(sourceList[i]))
                    return true;
            return false;
        }

        public bool Any()
        {
            var sourceList = source.Execute();
            return sourceList.Length > 0;
        }
    }
}
