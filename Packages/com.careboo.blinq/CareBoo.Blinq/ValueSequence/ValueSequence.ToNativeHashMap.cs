using System;
using Unity.Collections;
using CareBoo.Burst.Delegates;
using Unity.Jobs;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public struct ToNativeHashMapJob<T, TSource, TKey, TElement, TKeySelector, TElementSelector> : IJob
            where T : struct
            where TSource : struct, ISequence<T>
            where TKey : struct, IEquatable<TKey>
            where TElement : struct
            where TKeySelector : struct, IFunc<T, TKey>
            where TElementSelector : struct, IFunc<T, TElement>
        {
            [ReadOnly]
            ValueSequence<T, TSource> source;

            [ReadOnly]
            ValueFunc<T, TKey>.Struct<TKeySelector> keySelector;

            [ReadOnly]
            ValueFunc<T, TElement>.Struct<TElementSelector> elementSelector;

            [WriteOnly]
            NativeHashMap<TKey, TElement> output;

            public ToNativeHashMapJob(
                ValueSequence<T, TSource> source,
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

        public struct ToNativeHashMapJob<T, TSource, TKey, TKeySelector> : IJob
            where T : struct
            where TSource : struct, ISequence<T>
            where TKey : struct, IEquatable<TKey>
            where TKeySelector : struct, IFunc<T, TKey>
        {
            [ReadOnly]
            ValueSequence<T, TSource> source;

            [ReadOnly]
            ValueFunc<T, TKey>.Struct<TKeySelector> keySelector;

            [WriteOnly]
            NativeHashMap<TKey, T> output;

            public ToNativeHashMapJob(
                ValueSequence<T, TSource> source,
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
            var result = new NativeHashMap<TKey, TElement>(0, allocator);
            return source.ToNativeHashMap(keySelector, elementSelector, ref result);
        }

        public static NativeHashMap<TKey, TElement> RunToNativeHashMap<T, TSource, TKey, TElement, TKeySelector, TElementSelector>(
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
            var result = new NativeHashMap<TKey, TElement>(0, allocator);
            new ToNativeHashMapJob<T, TSource, TKey, TElement, TKeySelector, TElementSelector>(source, keySelector, elementSelector, ref result).Run();
            return result;
        }

        public static CollectionJobHandle<NativeHashMap<TKey, TElement>> ScheduleToNativeHashMap<T, TSource, TKey, TElement, TKeySelector, TElementSelector>(
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
            var result = new NativeHashMap<TKey, TElement>(0, allocator);
            var jobHandle = new ToNativeHashMapJob<T, TSource, TKey, TElement, TKeySelector, TElementSelector>(source, keySelector, elementSelector, ref result).Schedule();
            return new CollectionJobHandle<NativeHashMap<TKey, TElement>>(jobHandle, result);
        }

        public static NativeHashMap<TKey, TElement> ToNativeHashMap<T, TSource, TKey, TElement, TKeySelector, TElementSelector>(
            this in ValueSequence<T, TSource> source,
            in ValueFunc<T, TKey>.Struct<TKeySelector> keySelector,
            in ValueFunc<T, TElement>.Struct<TElementSelector> elementSelector,
            ref NativeHashMap<TKey, TElement> hashMap
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TKey : struct, IEquatable<TKey>
            where TElement : struct
            where TKeySelector : struct, IFunc<T, TKey>
            where TElementSelector : struct, IFunc<T, TElement>
        {
            var list = source.ToNativeList(Allocator.Temp);
            hashMap.Capacity = list.Length;
            for (var i = 0; i < list.Length; i++)
            {
                var key = keySelector.Invoke(list[i]);
                var val = elementSelector.Invoke(list[i]);
                hashMap.Add(key, val);
            }
            list.Dispose();
            return hashMap;
        }

        public static NativeHashMap<TKey, TElement> RunToNativeHashMap<T, TSource, TKey, TElement, TKeySelector, TElementSelector>(
            this in ValueSequence<T, TSource> source,
            in ValueFunc<T, TKey>.Struct<TKeySelector> keySelector,
            in ValueFunc<T, TElement>.Struct<TElementSelector> elementSelector,
            ref NativeHashMap<TKey, TElement> hashMap
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TKey : struct, IEquatable<TKey>
            where TElement : struct
            where TKeySelector : struct, IFunc<T, TKey>
            where TElementSelector : struct, IFunc<T, TElement>
        {
            new ToNativeHashMapJob<T, TSource, TKey, TElement, TKeySelector, TElementSelector>(source, keySelector, elementSelector, ref hashMap).Run();
            return hashMap;
        }

        public static JobHandle ScheduleToNativeHashMap<T, TSource, TKey, TElement, TKeySelector, TElementSelector>(
            this in ValueSequence<T, TSource> source,
            in ValueFunc<T, TKey>.Struct<TKeySelector> keySelector,
            in ValueFunc<T, TElement>.Struct<TElementSelector> elementSelector,
            ref NativeHashMap<TKey, TElement> hashMap
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TKey : struct, IEquatable<TKey>
            where TElement : struct
            where TKeySelector : struct, IFunc<T, TKey>
            where TElementSelector : struct, IFunc<T, TElement>
        {
            return new ToNativeHashMapJob<T, TSource, TKey, TElement, TKeySelector, TElementSelector>(source, keySelector, elementSelector, ref hashMap).Schedule();
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
            var result = new NativeHashMap<TKey, T>(0, allocator);
            return source.ToNativeHashMap(keySelector, ref result);
        }

        public static NativeHashMap<TKey, T> RunToNativeHashMap<T, TSource, TKey, TKeySelector>(
            this in ValueSequence<T, TSource> source,
            in ValueFunc<T, TKey>.Struct<TKeySelector> keySelector,
            in Allocator allocator
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TKey : struct, IEquatable<TKey>
            where TKeySelector : struct, IFunc<T, TKey>
        {
            var result = new NativeHashMap<TKey, T>(0, allocator);
            new ToNativeHashMapJob<T, TSource, TKey, TKeySelector>(source, keySelector, ref result).Run();
            return result;
        }

        public static CollectionJobHandle<NativeHashMap<TKey, T>> ScheduleToNativeHashMap<T, TSource, TKey, TKeySelector>(
            this in ValueSequence<T, TSource> source,
            in ValueFunc<T, TKey>.Struct<TKeySelector> keySelector,
            in Allocator allocator
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TKey : struct, IEquatable<TKey>
            where TKeySelector : struct, IFunc<T, TKey>
        {
            var result = new NativeHashMap<TKey, T>(0, allocator);
            var jobHandle = new ToNativeHashMapJob<T, TSource, TKey, TKeySelector>(source, keySelector, ref result).Schedule();
            return new CollectionJobHandle<NativeHashMap<TKey, T>>(jobHandle, result);
        }

        public static NativeHashMap<TKey, T> ToNativeHashMap<T, TSource, TKey, TKeySelector>(
            this in ValueSequence<T, TSource> source,
            in ValueFunc<T, TKey>.Struct<TKeySelector> keySelector,
            ref NativeHashMap<TKey, T> hashMap
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TKey : struct, IEquatable<TKey>
            where TKeySelector : struct, IFunc<T, TKey>
        {
            var list = source.ToNativeList(Allocator.Temp);
            hashMap.Capacity = list.Length;
            for (var i = 0; i < list.Length; i++)
            {
                var key = keySelector.Invoke(list[i]);
                var val = list[i];
                hashMap.Add(key, val);
            }
            list.Dispose();
            return hashMap;
        }

        public static NativeHashMap<TKey, T> RunToNativeHashMap<T, TSource, TKey, TKeySelector>(
            this in ValueSequence<T, TSource> source,
            in ValueFunc<T, TKey>.Struct<TKeySelector> keySelector,
            ref NativeHashMap<TKey, T> hashMap
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TKey : struct, IEquatable<TKey>
            where TKeySelector : struct, IFunc<T, TKey>
        {
            new ToNativeHashMapJob<T, TSource, TKey, TKeySelector>(source, keySelector, ref hashMap).Run();
            return hashMap;
        }

        public static JobHandle ScheduleToNativeHashMap<T, TSource, TKey, TKeySelector>(
            this in ValueSequence<T, TSource> source,
            in ValueFunc<T, TKey>.Struct<TKeySelector> keySelector,
            ref NativeHashMap<TKey, T> hashMap
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TKey : struct, IEquatable<TKey>
            where TKeySelector : struct, IFunc<T, TKey>
        {
            return new ToNativeHashMapJob<T, TSource, TKey, TKeySelector>(source, keySelector, ref hashMap).Schedule();
        }
    }
}
