using System;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static NativeHashMap<TKey, TElement> ToNativeHashMap<T, TSource, TKey, TElement, TKeySelector, TElementSelector>(
            this ref ValueSequence<T, TSource> source,
            ValueFunc<T, TKey>.Impl<TKeySelector> keySelector,
            ValueFunc<T, TElement>.Impl<TElementSelector> elementSelector,
            Allocator allocator
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
            this ref ValueSequence<T, TSource> source,
            ValueFunc<T, TKey>.Impl<TKeySelector> keySelector,
            Allocator allocator
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
