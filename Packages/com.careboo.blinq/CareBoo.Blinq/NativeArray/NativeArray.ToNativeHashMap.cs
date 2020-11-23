using System;
using Unity.Collections;
using CareBoo.Burst.Delegates;
using Unity.Jobs;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public struct ToNativeHashMapJob<T, TKey, TElement, TKeySelector, TElementSelector> : IJob
            where T : struct
            where TKey : struct, IEquatable<TKey>
            where TElement : struct
            where TKeySelector : struct, IFunc<T, TKey>
            where TElementSelector : struct, IFunc<T, TElement>
        {
            [ReadOnly]
            NativeArray<T> source;

            [ReadOnly]
            ValueFunc<T, TKey>.Struct<TKeySelector> keySelector;

            [ReadOnly]
            ValueFunc<T, TElement>.Struct<TElementSelector> elementSelector;

            [WriteOnly]
            NativeHashMap<TKey, TElement> output;

            public ToNativeHashMapJob(
                NativeArray<T> source,
                ValueFunc<T, TKey>.Struct<TKeySelector> keySelector,
                ValueFunc<T, TElement>.Struct<TElementSelector> elementSelector,
                ref NativeHashMap<TKey, TElement> output
                )
            {
                this.source = source;
                this.keySelector = keySelector;
                this.elementSelector = elementSelector;
                this.output = output;
            }

            public void Execute()
            {
                source.ToNativeHashMap(keySelector, elementSelector, ref output);
            }
        }

        public struct ToNativeHashMapJob<T, TKey, TKeySelector> : IJob
            where T : struct
            where TKey : struct, IEquatable<TKey>
            where TKeySelector : struct, IFunc<T, TKey>
        {
            [ReadOnly]
            NativeArray<T> source;

            [ReadOnly]
            ValueFunc<T, TKey>.Struct<TKeySelector> keySelector;

            [WriteOnly]
            NativeHashMap<TKey, T> output;

            public ToNativeHashMapJob(
                NativeArray<T> source,
                ValueFunc<T, TKey>.Struct<TKeySelector> keySelector,
                ref NativeHashMap<TKey, T> output
                )
            {
                this.source = source;
                this.keySelector = keySelector;
                this.output = output;
            }

            public void Execute()
            {
                source.ToNativeHashMap(keySelector, ref output);
            }
        }

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
            return source.ToNativeHashMap(keySelector, elementSelector, ref result);
        }

        public static NativeHashMap<TKey, TElement> RunToNativeHashMap<T, TKey, TElement, TKeySelector, TElementSelector>(
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
            return source.RunToNativeHashMap(keySelector, elementSelector, ref result);
        }

        public static CollectionJobHandle<NativeHashMap<TKey, TElement>> ScheduleToNativeHashMap<T, TKey, TElement, TKeySelector, TElementSelector>(
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
            var jobHandle = new ToNativeHashMapJob<T, TKey, TElement, TKeySelector, TElementSelector>(source, keySelector, elementSelector, ref result).Schedule();
            return new CollectionJobHandle<NativeHashMap<TKey, TElement>>(jobHandle, result);
        }

        public static NativeHashMap<TKey, TElement> ToNativeHashMap<T, TKey, TElement, TKeySelector, TElementSelector>(
            this in NativeArray<T> source,
            in ValueFunc<T, TKey>.Struct<TKeySelector> keySelector,
            in ValueFunc<T, TElement>.Struct<TElementSelector> elementSelector,
            ref NativeHashMap<TKey, TElement> hashMap
            )
            where T : struct
            where TKey : struct, IEquatable<TKey>
            where TElement : struct
            where TKeySelector : struct, IFunc<T, TKey>
            where TElementSelector : struct, IFunc<T, TElement>
        {
            hashMap.Capacity = source.Length;
            for (var i = 0; i < source.Length; i++)
            {
                var item = source[i];
                var key = keySelector.Invoke(item);
                var val = elementSelector.Invoke(item);
                hashMap.Add(key, val);
            }
            return hashMap;
        }

        public static NativeHashMap<TKey, TElement> RunToNativeHashMap<T, TKey, TElement, TKeySelector, TElementSelector>(
            this in NativeArray<T> source,
            in ValueFunc<T, TKey>.Struct<TKeySelector> keySelector,
            in ValueFunc<T, TElement>.Struct<TElementSelector> elementSelector,
            ref NativeHashMap<TKey, TElement> hashMap
            )
            where T : struct
            where TKey : struct, IEquatable<TKey>
            where TElement : struct
            where TKeySelector : struct, IFunc<T, TKey>
            where TElementSelector : struct, IFunc<T, TElement>
        {
            new ToNativeHashMapJob<T, TKey, TElement, TKeySelector, TElementSelector>(source, keySelector, elementSelector, ref hashMap).Run();
            return hashMap;
        }

        public static JobHandle ScheduleToNativeHashMap<T, TKey, TElement, TKeySelector, TElementSelector>(
            this in NativeArray<T> source,
            in ValueFunc<T, TKey>.Struct<TKeySelector> keySelector,
            in ValueFunc<T, TElement>.Struct<TElementSelector> elementSelector,
            ref NativeHashMap<TKey, TElement> hashMap
            )
            where T : struct
            where TKey : struct, IEquatable<TKey>
            where TElement : struct
            where TKeySelector : struct, IFunc<T, TKey>
            where TElementSelector : struct, IFunc<T, TElement>
        {
            return new ToNativeHashMapJob<T, TKey, TElement, TKeySelector, TElementSelector>(source, keySelector, elementSelector, ref hashMap).Schedule();
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
            return source.ToNativeHashMap(keySelector, ref result);
        }

        public static NativeHashMap<TKey, T> RunToNativeHashMap<T, TKey, TKeySelector>(
            this in NativeArray<T> source,
            in ValueFunc<T, TKey>.Struct<TKeySelector> keySelector,
            in Allocator allocator
            )
            where T : struct
            where TKey : struct, IEquatable<TKey>
            where TKeySelector : struct, IFunc<T, TKey>
        {
            var result = new NativeHashMap<TKey, T>(source.Length, allocator);
            return source.RunToNativeHashMap(keySelector, ref result);
        }

        public static CollectionJobHandle<NativeHashMap<TKey, T>> ScheduleToNativeHashMap<T, TKey, TKeySelector>(
            this in NativeArray<T> source,
            in ValueFunc<T, TKey>.Struct<TKeySelector> keySelector,
            in Allocator allocator
            )
            where T : struct
            where TKey : struct, IEquatable<TKey>
            where TKeySelector : struct, IFunc<T, TKey>
        {
            var result = new NativeHashMap<TKey, T>(source.Length, allocator);
            var jobHandle = new ToNativeHashMapJob<T, TKey, TKeySelector>(source, keySelector, ref result).Schedule();
            return new CollectionJobHandle<NativeHashMap<TKey, T>>(jobHandle, result);
        }

        public static NativeHashMap<TKey, T> ToNativeHashMap<T, TKey, TKeySelector>(
            this in NativeArray<T> source,
            in ValueFunc<T, TKey>.Struct<TKeySelector> keySelector,
            ref NativeHashMap<TKey, T> hashMap
            )
            where T : struct
            where TKey : struct, IEquatable<TKey>
            where TKeySelector : struct, IFunc<T, TKey>
        {
            hashMap.Capacity = source.Length;
            for (var i = 0; i < source.Length; i++)
            {
                var item = source[i];
                var key = keySelector.Invoke(item);
                hashMap.Add(key, item);
            }
            return hashMap;
        }

        public static NativeHashMap<TKey, T> RunToNativeHashMap<T, TKey, TKeySelector>(
            this in NativeArray<T> source,
            in ValueFunc<T, TKey>.Struct<TKeySelector> keySelector,
            ref NativeHashMap<TKey, T> hashMap
            )
            where T : struct
            where TKey : struct, IEquatable<TKey>
            where TKeySelector : struct, IFunc<T, TKey>
        {
            new ToNativeHashMapJob<T, TKey, TKeySelector>(source, keySelector, ref hashMap).Run();
            return hashMap;
        }

        public static JobHandle ScheduleToNativeHashMap<T, TKey, TKeySelector>(
            this in NativeArray<T> source,
            in ValueFunc<T, TKey>.Struct<TKeySelector> keySelector,
            ref NativeHashMap<TKey, T> hashMap
            )
            where T : struct
            where TKey : struct, IEquatable<TKey>
            where TKeySelector : struct, IFunc<T, TKey>
        {
            return new ToNativeHashMapJob<T, TKey, TKeySelector>(source, keySelector, ref hashMap).Schedule();
        }
    }
}
