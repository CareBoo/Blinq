using System.Collections.Generic;
using CareBoo.Burst.Delegates;
using Unity.Collections;
using Unity.Jobs;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static TResult Aggregate<T, TSource, TSourceEnumerator, TAccumulate, TResult, TFunc, TResultSelector>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            TAccumulate seed,
            ValueFunc<TAccumulate, T, TAccumulate>.Struct<TFunc> func,
            ValueFunc<TAccumulate, TResult>.Struct<TResultSelector> resultSelector
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TAccumulate : struct
            where TResult : struct
            where TFunc : struct, IFunc<TAccumulate, T, TAccumulate>
            where TResultSelector : struct, IFunc<TAccumulate, TResult>
        {
            var sourceList = source.ToNativeList(Allocator.Temp);
            for (var i = 0; i < sourceList.Length; i++)
                seed = func.Invoke(seed, sourceList[i]);
            var result = resultSelector.Invoke(seed);
            sourceList.Dispose();
            return result;
        }

        public struct SequenceAggregateFunc<T, TSource, TSourceEnumerator, TAccumulate, TResult, TFunc, TResultSelector>
            : IFunc<ValueSequence<T, TSource, TSourceEnumerator>, TResult>
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TAccumulate : struct
            where TResult : struct
            where TFunc : struct, IFunc<TAccumulate, T, TAccumulate>
            where TResultSelector : struct, IFunc<TAccumulate, TResult>
        {
            public TAccumulate Seed;
            public ValueFunc<TAccumulate, T, TAccumulate>.Struct<TFunc> Func;
            public ValueFunc<TAccumulate, TResult>.Struct<TResultSelector> ResultSelector;

            public TResult Invoke(ValueSequence<T, TSource, TSourceEnumerator> seq)
            {
                return seq.Aggregate(Seed, Func, ResultSelector);
            }
        }

        public static ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, TResult>.Struct<SequenceAggregateFunc<T, TSource, TSourceEnumerator, TAccumulate, TResult, TFunc, TResultSelector>>
        AggregateAsFunc<T, TSource, TSourceEnumerator, TAccumulate, TResult, TFunc, TResultSelector>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            TAccumulate seed,
            ValueFunc<TAccumulate, T, TAccumulate>.Struct<TFunc> func,
            ValueFunc<TAccumulate, TResult>.Struct<TResultSelector> resultSelector
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TAccumulate : struct
            where TResult : struct
            where TFunc : struct, IFunc<TAccumulate, T, TAccumulate>
            where TResultSelector : struct, IFunc<TAccumulate, TResult>
        {
            var seqAggregateFunc = new SequenceAggregateFunc<T, TSource, TSourceEnumerator, TAccumulate, TResult, TFunc, TResultSelector> { Seed = seed, Func = func, ResultSelector = resultSelector };
            return ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, TResult>.New(seqAggregateFunc);
        }

        public static TResult RunAggregate<T, TSource, TSourceEnumerator, TAccumulate, TResult, TFunc, TResultSelector>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            TAccumulate seed,
            ValueFunc<TAccumulate, T, TAccumulate>.Struct<TFunc> func,
            ValueFunc<TAccumulate, TResult>.Struct<TResultSelector> resultSelector
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TAccumulate : struct
            where TResult : struct
            where TFunc : struct, IFunc<TAccumulate, T, TAccumulate>
            where TResultSelector : struct, IFunc<TAccumulate, TResult>
        {
            var aggregateFunc = source.AggregateAsFunc(seed, func, resultSelector);
            return source.Run(aggregateFunc);
        }

        public static JobHandle<TResult> ScheduleAggregate<T, TSource, TSourceEnumerator, TAccumulate, TResult, TFunc, TResultSelector>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            TAccumulate seed,
            ValueFunc<TAccumulate, T, TAccumulate>.Struct<TFunc> func,
            ValueFunc<TAccumulate, TResult>.Struct<TResultSelector> resultSelector
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TAccumulate : struct
            where TResult : struct
            where TFunc : struct, IFunc<TAccumulate, T, TAccumulate>
            where TResultSelector : struct, IFunc<TAccumulate, TResult>
        {
            var aggregateFunc = source.AggregateAsFunc(seed, func, resultSelector);
            return source.Schedule(aggregateFunc);
        }

        public static JobHandle ScheduleAggregate<T, TSource, TSourceEnumerator, TAccumulate, TResult, TFunc, TResultSelector>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ref NativeArray<TResult> output,
            TAccumulate seed,
            ValueFunc<TAccumulate, T, TAccumulate>.Struct<TFunc> func,
            ValueFunc<TAccumulate, TResult>.Struct<TResultSelector> resultSelector
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TAccumulate : struct
            where TResult : struct
            where TFunc : struct, IFunc<TAccumulate, T, TAccumulate>
            where TResultSelector : struct, IFunc<TAccumulate, TResult>
        {
            var aggregateFunc = source.AggregateAsFunc(seed, func, resultSelector);
            return source.Schedule(aggregateFunc, ref output);
        }

        public static TAccumulate Aggregate<T, TSource, TSourceEnumerator, TAccumulate, TFunc>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            TAccumulate seed,
            ValueFunc<TAccumulate, T, TAccumulate>.Struct<TFunc> func
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TAccumulate : struct
            where TFunc : struct, IFunc<TAccumulate, T, TAccumulate>
        {
            var sourceList = source.ToNativeList(Allocator.Temp);
            for (var i = 0; i < sourceList.Length; i++)
                seed = func.Invoke(seed, sourceList[i]);
            sourceList.Dispose();
            return seed;
        }

        public struct SequenceAggregateFunc<T, TSource, TSourceEnumerator, TAccumulate, TFunc>
            : IFunc<ValueSequence<T, TSource, TSourceEnumerator>, TAccumulate>
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TAccumulate : struct
            where TFunc : struct, IFunc<TAccumulate, T, TAccumulate>
        {
            public TAccumulate Seed;
            public ValueFunc<TAccumulate, T, TAccumulate>.Struct<TFunc> Func;

            public TAccumulate Invoke(ValueSequence<T, TSource, TSourceEnumerator> seq)
            {
                return seq.Aggregate(Seed, Func);
            }
        }

        public static ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, TAccumulate>.Struct<SequenceAggregateFunc<T, TSource, TSourceEnumerator, TAccumulate, TFunc>>
        AggregateAsFunc<T, TSource, TSourceEnumerator, TAccumulate, TFunc>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            TAccumulate seed,
            ValueFunc<TAccumulate, T, TAccumulate>.Struct<TFunc> func
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TAccumulate : struct
            where TFunc : struct, IFunc<TAccumulate, T, TAccumulate>
        {
            var seqAggregateFunc = new SequenceAggregateFunc<T, TSource, TSourceEnumerator, TAccumulate, TFunc> { Seed = seed, Func = func };
            return ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, TAccumulate>.New(seqAggregateFunc);
        }

        public static TAccumulate RunAggregate<T, TSource, TSourceEnumerator, TAccumulate, TFunc>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            TAccumulate seed,
            ValueFunc<TAccumulate, T, TAccumulate>.Struct<TFunc> func
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TAccumulate : struct
            where TFunc : struct, IFunc<TAccumulate, T, TAccumulate>
        {
            var aggregateFunc = source.AggregateAsFunc(seed, func);
            return source.Run(aggregateFunc);
        }

        public static JobHandle<TAccumulate> ScheduleAggregate<T, TSource, TSourceEnumerator, TAccumulate, TFunc>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            TAccumulate seed,
            ValueFunc<TAccumulate, T, TAccumulate>.Struct<TFunc> func
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TAccumulate : struct
            where TFunc : struct, IFunc<TAccumulate, T, TAccumulate>
        {
            var aggregateFunc = source.AggregateAsFunc(seed, func);
            return source.Schedule(aggregateFunc);
        }

        public static JobHandle ScheduleAggregate<T, TSource, TSourceEnumerator, TAccumulate, TFunc>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ref NativeArray<TAccumulate> output,
            TAccumulate seed,
            ValueFunc<TAccumulate, T, TAccumulate>.Struct<TFunc> func
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TAccumulate : struct
            where TFunc : struct, IFunc<TAccumulate, T, TAccumulate>
        {
            var aggregateFunc = source.AggregateAsFunc(seed, func);
            return source.Schedule(aggregateFunc, ref output);
        }

        public static T Aggregate<T, TSource, TSourceEnumerator, TFunc>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, T, T>.Struct<TFunc> func
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TFunc : struct, IFunc<T, T, T>
        {
            var sourceList = source.ToNativeList(Allocator.Temp);
            if (sourceList.Length == 0)
            {
                sourceList.Dispose();
                throw Error.NoElements();
            }
            var seed = sourceList[0];
            for (var i = 1; i < sourceList.Length; i++)
                seed = func.Invoke(seed, sourceList[i]);
            sourceList.Dispose();
            return seed;
        }

        public struct SequenceAggregateFunc<T, TSource, TSourceEnumerator, TFunc>
            : IFunc<ValueSequence<T, TSource, TSourceEnumerator>, T>
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TFunc : struct, IFunc<T, T, T>
        {
            public ValueFunc<T, T, T>.Struct<TFunc> Func;

            public T Invoke(ValueSequence<T, TSource, TSourceEnumerator> seq)
            {
                return seq.Aggregate(Func);
            }
        }

        public static ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, T>.Struct<SequenceAggregateFunc<T, TSource, TSourceEnumerator, TFunc>>
        AggregateAsFunc<T, TSource, TSourceEnumerator, TFunc>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, T, T>.Struct<TFunc> func
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TFunc : struct, IFunc<T, T, T>
        {
            var seqAggregateFunc = new SequenceAggregateFunc<T, TSource, TSourceEnumerator, TFunc> { Func = func };
            return ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, T>.New(seqAggregateFunc);
        }

        public static T RunAggregate<T, TSource, TSourceEnumerator, TFunc>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, T, T>.Struct<TFunc> func
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TFunc : struct, IFunc<T, T, T>
        {
            var aggregateFunc = source.AggregateAsFunc(func);
            return source.Run(aggregateFunc);
        }

        public static JobHandle<T> ScheduleAggregate<T, TSource, TSourceEnumerator, TFunc>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, T, T>.Struct<TFunc> func
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TFunc : struct, IFunc<T, T, T>
        {
            var aggregateFunc = source.AggregateAsFunc(func);
            return source.Schedule(aggregateFunc);
        }

        public static JobHandle ScheduleAggregate<T, TSource, TSourceEnumerator, TFunc>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ref NativeArray<T> output,
            ValueFunc<T, T, T>.Struct<TFunc> func
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TFunc : struct, IFunc<T, T, T>
        {
            var aggregateFunc = source.AggregateAsFunc(func);
            return source.Schedule(aggregateFunc, ref output);
        }
    }
}
