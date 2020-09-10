using System.Collections.Generic;

namespace CareBoo.Blinq
{
    public partial struct ValueSequence<T, TSource>
    {
        public bool Contains(T item)
        {
            var sourceList = Execute();
            for (var i = 0; i < sourceList.Length; i++)
                if (sourceList[i].Equals(item))
                    return true;
            return false;
        }

        public bool Contains<TComparer>(T item, TComparer comparer)
            where TComparer : struct, IEqualityComparer<T>
        {
            var sourceList = Execute();
            for (var i = 0; i < sourceList.Length; i++)
                if (comparer.Equals(sourceList[i], item))
                    return true;
            return false;
        }
    }
}
