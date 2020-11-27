using System;
using Unity.Collections;
using CareBoo.Burst.Delegates;
using Unity.Jobs;
using System.Collections.Generic;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public struct SequenceToNativeHashMapJob<T, TSource, TSourceEnumerator, TKey, TElement, TKeySelector, TElementSelector> : IJob
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TKey : struct, IEquatable<TKey>
            where TElement : struct
            where TKeySelector : struct, IFunc<T, TKey>
            where TElementSelector : struct, IFunc<T, TElement>
        {
            [ReadOnly]
            ValueSequence<T, TSource, TSourceEnumerator> source;

            [ReadOnly]
            ValueFunc<T, TKey>.Struct<TKeySelector> keySelector;

            [ReadOnly]
            ValueFunc<T, TElement>.Struct<TElementSelector> elementSelector;

            [WriteOnly]
            NativeHashMap<TKey, TElement> output;

            public SequenceToNativeHashMapJob(
                ValueSequence<T, TSource, TSourceEnumerator> source,
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

        public struct SequenceToNativeHashMapJob<T, TSource, TSourceEnumerator, TKey, TKeySelector> : IJob
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TKey : struct, IEquatable<TKey>
            where TKeySelector : struct, IFunc<T, TKey>
        {
            [ReadOnly]
            ValueSequence<T, TSource, TSourceEnumerator> source;

            [ReadOnly]
            ValueFunc<T, TKey>.Struct<TKeySelector> keySelector;

            [WriteOnly]
            NativeHashMap<TKey, T> output;

            public SequenceToNativeHashMapJob(
                ValueSequence<T, TSource, TSourceEnumerator> source,
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

        public static NativeHashMap<TKey, TElement> ToNativeHashMap<T, TSource, TSourceEnumerator, TKey, TElement, TKeySelector, TElementSelector>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, TKey>.Struct<TKeySelector> keySelector,
            ValueFunc<T, TElement>.Struct<TElementSelector> elementSelector,
            in Allocator allocator
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TKey : struct, IEquatable<TKey>
            where TElement : struct
            where TKeySelector : struct, IFunc<T, TKey>
            where TElementSelector : struct, IFunc<T, TElement>
        {
            var result = new NativeHashMap<TKey, TElement>(0, allocator);
            return source.ToNativeHashMap(keySelector, elementSelector, ref result);
        }

        public static NativeHashMap<TKey, TElement> RunToNativeHashMap<T, TSource, TSourceEnumerator, TKey, TElement, TKeySelector, TElementSelector>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, TKey>.Struct<TKeySelector> keySelector,
            ValueFunc<T, TElement>.Struct<TElementSelector> elementSelector,
            in Allocator allocator
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TKey : struct, IEquatable<TKey>
            where TElement : struct
            where TKeySelector : struct, IFunc<T, TKey>
            where TElementSelector : struct, IFunc<T, TElement>
        {
            var result = new NativeHashMap<TKey, TElement>(0, allocator);
            new SequenceToNativeHashMapJob<T, TSource, TSourceEnumerator, TKey, TElement, TKeySelector, TElementSelector>(source, keySelector, elementSelector, ref result).Run();
            return result;
        }

        public static CollectionJobHandle<NativeHashMap<TKey, TElement>> ScheduleToNativeHashMap<T, TSource, TSourceEnumerator, TKey, TElement, TKeySelector, TElementSelector>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, TKey>.Struct<TKeySelector> keySelector,
            ValueFunc<T, TElement>.Struct<TElementSelector> elementSelector,
            in Allocator allocator
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TKey : struct, IEquatable<TKey>
            where TElement : struct
            where TKeySelector : struct, IFunc<T, TKey>
            where TElementSelector : struct, IFunc<T, TElement>
        {
            var result = new NativeHashMap<TKey, TElement>(0, allocator);
            var jobHandle = new SequenceToNativeHashMapJob<T, TSource, TSourceEnumerator, TKey, TElement, TKeySelector, TElementSelector>(source, keySelector, elementSelector, ref result).Schedule();
            return new CollectionJobHandle<NativeHashMap<TKey, TElement>>(jobHandle, result);
        }

        public static NativeHashMap<TKey, TElement> ToNativeHashMap<T, TSource, TSourceEnumerator, TKey, TElement, TKeySelector, TElementSelector>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, TKey>.Struct<TKeySelector> keySelector,
            ValueFunc<T, TElement>.Struct<TElementSelector> elementSelector,
            ref NativeHashMap<TKey, TElement> hashMap
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
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

        public static NativeHashMap<TKey, TElement> RunToNativeHashMap<T, TSource, TSourceEnumerator, TKey, TElement, TKeySelector, TElementSelector>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, TKey>.Struct<TKeySelector> keySelector,
            ValueFunc<T, TElement>.Struct<TElementSelector> elementSelector,
            ref NativeHashMap<TKey, TElement> hashMap
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TKey : struct, IEquatable<TKey>
            where TElement : struct
            where TKeySelector : struct, IFunc<T, TKey>
            where TElementSelector : struct, IFunc<T, TElement>
        {
            new SequenceToNativeHashMapJob<T, TSource, TSourceEnumerator, TKey, TElement, TKeySelector, TElementSelector>(source, keySelector, elementSelector, ref hashMap).Run();
            return hashMap;
        }

        public static JobHandle ScheduleToNativeHashMap<T, TSource, TSourceEnumerator, TKey, TElement, TKeySelector, TElementSelector>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, TKey>.Struct<TKeySelector> keySelector,
            ValueFunc<T, TElement>.Struct<TElementSelector> elementSelector,
            ref NativeHashMap<TKey, TElement> hashMap
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TKey : struct, IEquatable<TKey>
            where TElement : struct
            where TKeySelector : struct, IFunc<T, TKey>
            where TElementSelector : struct, IFunc<T, TElement>
        {
            return new SequenceToNativeHashMapJob<T, TSource, TSourceEnumerator, TKey, TElement, TKeySelector, TElementSelector>(source, keySelector, elementSelector, ref hashMap).Schedule();
        }

        public static NativeHashMap<TKey, T> ToNativeHashMap<T, TSource, TSourceEnumerator, TKey, TKeySelector>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, TKey>.Struct<TKeySelector> keySelector,
            in Allocator allocator
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TKey : struct, IEquatable<TKey>
            where TKeySelector : struct, IFunc<T, TKey>
        {
            var result = new NativeHashMap<TKey, T>(0, allocator);
            return source.ToNativeHashMap(keySelector, ref result);
        }

        public static NativeHashMap<TKey, T> RunToNativeHashMap<T, TSource, TSourceEnumerator, TKey, TKeySelector>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, TKey>.Struct<TKeySelector> keySelector,
            in Allocator allocator
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TKey : struct, IEquatable<TKey>
            where TKeySelector : struct, IFunc<T, TKey>
        {
            var result = new NativeHashMap<TKey, T>(0, allocator);
            new SequenceToNativeHashMapJob<T, TSource, TSourceEnumerator, TKey, TKeySelector>(source, keySelector, ref result).Run();
            return result;
        }

        public static CollectionJobHandle<NativeHashMap<TKey, T>> ScheduleToNativeHashMap<T, TSource, TSourceEnumerator, TKey, TKeySelector>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, TKey>.Struct<TKeySelector> keySelector,
            in Allocator allocator
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TKey : struct, IEquatable<TKey>
            where TKeySelector : struct, IFunc<T, TKey>
        {
            var result = new NativeHashMap<TKey, T>(0, allocator);
            var jobHandle = new SequenceToNativeHashMapJob<T, TSource, TSourceEnumerator, TKey, TKeySelector>(source, keySelector, ref result).Schedule();
            return new CollectionJobHandle<NativeHashMap<TKey, T>>(jobHandle, result);
        }

        public static NativeHashMap<TKey, T> ToNativeHashMap<T, TSource, TSourceEnumerator, TKey, TKeySelector>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, TKey>.Struct<TKeySelector> keySelector,
            ref NativeHashMap<TKey, T> hashMap
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
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

        public static NativeHashMap<TKey, T> RunToNativeHashMap<T, TSource, TSourceEnumerator, TKey, TKeySelector>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, TKey>.Struct<TKeySelector> keySelector,
            ref NativeHashMap<TKey, T> hashMap
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TKey : struct, IEquatable<TKey>
            where TKeySelector : struct, IFunc<T, TKey>
        {
            new SequenceToNativeHashMapJob<T, TSource, TSourceEnumerator, TKey, TKeySelector>(source, keySelector, ref hashMap).Run();
            return hashMap;
        }

        public static JobHandle ScheduleToNativeHashMap<T, TSource, TSourceEnumerator, TKey, TKeySelector>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, TKey>.Struct<TKeySelector> keySelector,
            ref NativeHashMap<TKey, T> hashMap
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TKey : struct, IEquatable<TKey>
            where TKeySelector : struct, IFunc<T, TKey>
        {
            return new SequenceToNativeHashMapJob<T, TSource, TSourceEnumerator, TKey, TKeySelector>(source, keySelector, ref hashMap).Schedule();
        }
    }
}
