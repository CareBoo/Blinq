using System;
using Unity.Collections;
using CareBoo.Burst.Delegates;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static NativeHashMap<TKey, TElement> ToNativeHashMap<T, TKey, TElement, TKeySelector, TElementSelector>(
            this in NativeArray<T> source,
            in ValueFunc<T, TKey>.Struct<TKeySelector> keySelector,
            in ValueFunc<T, TElement>.Struct<TElementSelector> elementSelector,
            in Allocator allocator
            )
            where T : struct
            where TKey : struct, IEquatable<TKey>
            where TElement : struct
            where TKeySelector : struct, IFunc<T, TKey>
            where TElementSelector : struct, IFunc<T, TElement>
        {
            var result = new NativeHashMap<TKey, TElement>(source.Length, allocator);
            for (var i = 0; i < source.Length; i++)
            {
                var item = source[i];
                var key = keySelector.Invoke(item);
                var val = elementSelector.Invoke(item);
                result.Add(key, val);
            }
            return result;
        }

        public static NativeHashMap<TKey, T> ToNativeHashMap<T, TKey, TKeySelector>(
            this in NativeArray<T> source,
            in ValueFunc<T, TKey>.Struct<TKeySelector> keySelector,
            in Allocator allocator
            )
            where T : struct
            where TKey : struct, IEquatable<TKey>
            where TKeySelector : struct, IFunc<T, TKey>
        {
            var result = new NativeHashMap<TKey, T>(source.Length, allocator);
            for (var i = 0; i < source.Length; i++)
            {
                var item = source[i];
                var key = keySelector.Invoke(item);
                result.Add(key, item);
            }
            return result;
        }
    }
}
