using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public struct ValueGrouping<TKey, TElement>
        : IGrouping<TKey, TElement>
        where TKey : struct, IEquatable<TKey>
        where TElement : struct
    {
        readonly TKey key;
        readonly NativeMultiHashMap<TKey, TElement> groupMap;

        public TKey Key => key;

        public ValueGrouping(in TKey key, in NativeMultiHashMap<TKey, TElement> groupMap)
        {
            this.key = key;
            this.groupMap = groupMap;
        }

        public NativeMultiHashMap<TKey, TElement>.Enumerator GetEnumerator()
        {
            return groupMap.GetValuesForKey(key);
        }

        IEnumerator<TElement> IEnumerable<TElement>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }

    public static class ValueGrouping
    {
        public static ValueGrouping<TKey, TElement> New<TKey, TElement>(
            in TKey key,
            in NativeMultiHashMap<TKey, TElement> groupMap
            )
            where TKey : struct, IEquatable<TKey>
            where TElement : struct
        {
            return new ValueGrouping<TKey, TElement>(key, groupMap);
        }
    }
}
