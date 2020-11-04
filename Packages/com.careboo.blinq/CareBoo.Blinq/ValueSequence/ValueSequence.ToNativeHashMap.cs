using System;
using Unity.Collections;
using CareBoo.Burst.Delegates;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static NativeHashMap<TKey, TElement> ToNativeHashMap<T, TSource, TKey, TElement, TKeySelector, TElementSelector>(
            this in ValueSequence<T, TSource> source,
            in ValueFunc<T, TKey>.Struct<TKeySelector> keySelector,
            in ValueFunc<T, TElement>.Struct<TElementSelector> elementSelector,
            in Allocator allocator
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TKey : struct, IEquatable<TKey>
            where TElement : struct
            where TKeySelector : struct, IFunc<T, TKey>
            where TElementSelector : struct, IFunc<T, TElement>
        {
            var list = source.Execute();
            var result = new NativeHashMap<TKey, TElement>(list.Length, allocator);
            for (var i = 0; i < list.Length; i++)
            {
                var key = keySelector.Invoke(list[i]);
                var val = elementSelector.Invoke(list[i]);
                result.Add(key, val);
            }
            list.Dispose();
            return result;
        }

        public static NativeHashMap<TKey, T> ToNativeHashMap<T, TSource, TKey, TKeySelector>(
            this in ValueSequence<T, TSource> source,
            in ValueFunc<T, TKey>.Struct<TKeySelector> keySelector,
            in Allocator allocator
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TKey : struct, IEquatable<TKey>
            where TKeySelector : struct, IFunc<T, TKey>
        {
            var list = source.Execute();
            var result = new NativeHashMap<TKey, T>(list.Length, allocator);
            for (var i = 0; i < list.Length; i++)
            {
                var key = keySelector.Invoke(list[i]);
                var val = list[i];
                result.Add(key, val);
            }
            list.Dispose();
            return result;
        }
    }
}
