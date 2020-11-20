using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace CareBoo.Blinq
{
    public struct ValueGrouping<TKey, TValue> : IGrouping<TKey, TValue>
        where TKey : struct, IEquatable<TKey>
        where TValue : struct
    {
        readonly TKey key;
        readonly ValueGroupingValues<TKey, TValue> values;

        public ValueGrouping(TKey key, ValueGroupingValues<TKey, TValue> values)
        {
            this.key = key;
            this.values = values;
        }

        public TKey Key => key;

        public UnsafeMultiHashMapEnumerator<TKey, TValue> GetEnumerator()
        {
            return values.GetEnumerator();
        }

        IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public struct ValueGroupingValues<TKey, TValue> : IEnumerable<TValue>
        where TKey : struct, IEquatable<TKey>
        where TValue : struct
    {
        readonly UnsafeMultiHashMapEnumerator<TKey, TValue> enumerator;

        public ValueGroupingValues(TKey key, UnsafeMultiHashMap<TKey, TValue> map)
        {
            this.enumerator = new UnsafeMultiHashMapEnumerator<TKey, TValue>(map, key);
        }

        public NativeList<TValue> ToNativeList(Allocator allocator)
        {
            var list = new NativeList<TValue>(allocator);
            var enumerator = GetEnumerator();
            while (enumerator.MoveNext())
                list.Add(enumerator.Current);
            return list;
        }

        public UnsafeMultiHashMapEnumerator<TKey, TValue> GetEnumerator()
        {
            return enumerator;
        }

        IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public struct UnsafeMultiHashMapEnumerator<TKey, TValue> : IEnumerator<TValue>
        where TValue : struct
        where TKey : struct, IEquatable<TKey>
    {
        readonly UnsafeMultiHashMap<TKey, TValue> map;
        readonly TKey key;

        bool isFirst;
        TValue value;
        NativeMultiHashMapIterator<TKey> iterator;

        public UnsafeMultiHashMapEnumerator(UnsafeMultiHashMap<TKey, TValue> map, TKey key)
        {
            this.map = map;
            this.key = key;
            iterator = default;
            value = default;
            isFirst = true;
        }

        public TValue Current => value;

        object IEnumerator.Current => Current;

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            if (isFirst)
            {
                isFirst = false;
                return map.TryGetFirstValue(key, out value, out iterator);
            }

            return map.TryGetNextValue(out value, ref iterator);
        }

        public void Reset()
        {
            isFirst = true;
        }
    }
}
