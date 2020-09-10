using System;
using Unity.Burst;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public partial struct ValueSequence<T, TSource>
        where T : struct
        where TSource : struct, ISequence<T>
    {
        [BurstCompile]
        public NativeHashMap<TKey, TElement> ToNativeHashMap<TKey, TElement, TKeySelector, TElementSelector>(
            TKeySelector keySelector,
            TElementSelector elementSelector,
            Allocator allocator
            )
            where TKey : struct, IEquatable<TKey>
            where TElement : struct
            where TKeySelector : struct, IValueFunc<T, TKey>
            where TElementSelector : struct, IValueFunc<T, TElement>
        {
            var output = new NativeHashMap<TKey, TElement>(0, allocator);
            ToNativeHashMap(keySelector, elementSelector, output);
            return output;
        }

        [BurstCompile]
        public void ToNativeHashMap<TKey, TElement, TKeySelector, TElementSelector>(
            TKeySelector keySelector,
            TElementSelector elementSelector,
            NativeHashMap<TKey, TElement> output
            )
            where TKey : struct, IEquatable<TKey>
            where TElement : struct
            where TKeySelector : struct, IValueFunc<T, TKey>
            where TElementSelector : struct, IValueFunc<T, TElement>
        {
            var list = Execute();
            output.Capacity = list.Length;
            for (var i = 0; i < list.Length; i++)
            {
                var key = keySelector.Invoke(list[i]);
                var element = elementSelector.Invoke(list[i]);
                if (!output.TryAdd(key, element))
                    throw Error.DuplicateKey();
            }
        }

        [BurstCompile]
        public NativeHashMap<TKey, T> ToNativeHashMap<TKey, TKeySelector>(
            TKeySelector keySelector,
            Allocator allocator
            )
            where TKey : struct, IEquatable<TKey>
            where TKeySelector : struct, IValueFunc<T, TKey>
        {
            var output = new NativeHashMap<TKey, T>(0, allocator);
            ToNativeHashMap(keySelector, output);
            return output;
        }

        [BurstCompile]
        public void ToNativeHashMap<TKey, TKeySelector>(
            TKeySelector keySelector,
            NativeHashMap<TKey, T> output
            )
            where TKey : struct, IEquatable<TKey>
            where TKeySelector : struct, IValueFunc<T, TKey>
        {
            var list = Execute();
            output.Capacity = list.Length;
            for (var i = 0; i < list.Length; i++)
            {
                var key = keySelector.Invoke(list[i]);
                var element = list[i];
                if (!output.TryAdd(key, element))
                    throw Error.DuplicateKey();
            }
        }
    }
}
