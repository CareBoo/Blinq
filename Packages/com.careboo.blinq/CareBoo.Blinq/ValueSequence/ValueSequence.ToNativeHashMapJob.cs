using System;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;

namespace CareBoo.Blinq
{
    public partial struct ValueSequence<T, TQuery>
        : IEnumerable<T>
        where T : struct
        where TQuery : struct, IQuery<T>
    {
        public NativeHashMapJob<TKey, TElement, TKeySelector, TElementSelector> ToNativeHashMapJob<TKey, TElement, TKeySelector, TElementSelector>(
            TKeySelector keySelector,
            TElementSelector elementSelector,
            NativeHashMap<TKey, TElement> output
            )
            where TKey : struct, IEquatable<TKey>
            where TElement : struct
            where TKeySelector : struct, IValueFunc<T, TKey>
            where TElementSelector : struct, IValueFunc<T, TElement>
        {
            return new NativeHashMapJob<TKey, TElement, TKeySelector, TElementSelector>
            {
                Sequence = this,
                KeySelector = keySelector,
                ElementSelector = elementSelector,
                Output = output
            };
        }

        public SequenceExecutionJobWrapper<KeyValue<TKey, TElement>, NativeHashMapJob<TKey, TElement, TKeySelector, TElementSelector>, NativeHashMap<TKey, TElement>> ToNativeHashMapJob<TKey, TElement, TKeySelector, TElementSelector>(
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
            var job = new NativeHashMapJob<TKey, TElement, TKeySelector, TElementSelector>
            {
                Sequence = this,
                KeySelector = keySelector,
                ElementSelector = elementSelector,
                Output = output
            };
            return new SequenceExecutionJobWrapper<KeyValue<TKey, TElement>, NativeHashMapJob<TKey, TElement, TKeySelector, TElementSelector>, NativeHashMap<TKey, TElement>>(job, output);
        }

        public NativeHashMapJob<TKey, TKeySelector> ToNativeHashMapJob<TKey, TKeySelector>(
            TKeySelector keySelector,
            NativeHashMap<TKey, T> output
            )
            where TKey : struct, IEquatable<TKey>
            where TKeySelector : struct, IValueFunc<T, TKey>
        {
            return new NativeHashMapJob<TKey, TKeySelector>
            {
                Sequence = this,
                KeySelector = keySelector,
                Output = output
            };
        }

        public SequenceExecutionJobWrapper<KeyValue<TKey, T>, NativeHashMapJob<TKey, TKeySelector>, NativeHashMap<TKey, T>> ToNativeHashMapJob<TKey, TKeySelector>(
            TKeySelector keySelector,
            Allocator allocator
            )
            where TKey : struct, IEquatable<TKey>
            where TKeySelector : struct, IValueFunc<T, TKey>
        {
            var output = new NativeHashMap<TKey, T>(0, allocator);
            var job = new NativeHashMapJob<TKey, TKeySelector>
            {
                Sequence = this,
                KeySelector = keySelector,
                Output = output
            };
            return new SequenceExecutionJobWrapper<KeyValue<TKey, T>, NativeHashMapJob<TKey, TKeySelector>, NativeHashMap<TKey, T>>(job, output);
        }

        [BurstCompile]
        public struct NativeHashMapJob<TKey, TElement, TKeySelector, TElementSelector> : IJob
            where TKey : struct, IEquatable<TKey>
            where TElement : struct
            where TKeySelector : struct, IValueFunc<T, TKey>
            where TElementSelector : struct, IValueFunc<T, TElement>
        {
            public ValueSequence<T, TQuery> Sequence;
            public TKeySelector KeySelector;
            public TElementSelector ElementSelector;

            [WriteOnly]
            public NativeHashMap<TKey, TElement> Output;

            public void Execute()
            {
                Sequence.ToNativeHashMap(KeySelector, ElementSelector, Output);
            }
        }

        [BurstCompile]
        public struct NativeHashMapJob<TKey, TKeySelector> : IJob
            where TKey : struct, IEquatable<TKey>
            where TKeySelector : struct, IValueFunc<T, TKey>
        {
            public ValueSequence<T, TQuery> Sequence;
            public TKeySelector KeySelector;

            [WriteOnly]
            public NativeHashMap<TKey, T> Output;

            public void Execute()
            {
                Sequence.ToNativeHashMap(KeySelector, Output);
            }
        }
    }
}
