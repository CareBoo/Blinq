using Unity.Collections;
using CareBoo.Burst.Delegates;
using Unity.Jobs;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static TResult Aggregate<T, TAccumulate, TResult, TFunc, TResultSelector>(
            this in NativeArray<T> source,
            TAccumulate seed,
            ValueFunc<TAccumulate, T, TAccumulate>.Struct<TFunc> func,
            ValueFunc<TAccumulate, TResult>.Struct<TResultSelector> resultSelector
            )
            where T : struct
            where TAccumulate : struct
            where TResult : struct
            where TFunc : struct, IFunc<TAccumulate, T, TAccumulate>
            where TResultSelector : struct, IFunc<TAccumulate, TResult>
        {
            for (var i = 0; i < source.Length; i++)
                seed = func.Invoke(seed, source[i]);
            return resultSelector.Invoke(seed);
        }

        public struct ArrayAggregateFunc<T, TAccumulate, TResult, TFunc, TResultSelector>
            : IFunc<NativeArray<T>, TResult>
            where T : struct
            where TAccumulate : struct
            where TResult : struct
            where TFunc : struct, IFunc<TAccumulate, T, TAccumulate>
            where TResultSelector : struct, IFunc<TAccumulate, TResult>
        {
            public TAccumulate Seed;
            public ValueFunc<TAccumulate, T, TAccumulate>.Struct<TFunc> Func;
            public ValueFunc<TAccumulate, TResult>.Struct<TResultSelector> ResultSelector;

            public TResult Invoke(NativeArray<T> seq)
            {
                return seq.Aggregate(Seed, Func, ResultSelector);
            }
        }

        public static ValueFunc<NativeArray<T>, TResult>.Struct<ArrayAggregateFunc<T, TAccumulate, TResult, TFunc, TResultSelector>>
        AggregateAsFunc<T, TAccumulate, TResult, TFunc, TResultSelector>(
            this in NativeArray<T> source,
            TAccumulate seed,
            ValueFunc<TAccumulate, T, TAccumulate>.Struct<TFunc> func,
            ValueFunc<TAccumulate, TResult>.Struct<TResultSelector> resultSelector
            )
            where T : struct
            where TAccumulate : struct
            where TResult : struct
            where TFunc : struct, IFunc<TAccumulate, T, TAccumulate>
            where TResultSelector : struct, IFunc<TAccumulate, TResult>
        {
            var seqAggregateFunc = new ArrayAggregateFunc<T, TAccumulate, TResult, TFunc, TResultSelector> { Seed = seed, Func = func, ResultSelector = resultSelector };
            return ValueFunc<NativeArray<T>, TResult>.New(seqAggregateFunc);
        }

        public static TResult RunAggregate<T, TAccumulate, TResult, TFunc, TResultSelector>(
            this in NativeArray<T> source,
            TAccumulate seed,
            ValueFunc<TAccumulate, T, TAccumulate>.Struct<TFunc> func,
            ValueFunc<TAccumulate, TResult>.Struct<TResultSelector> resultSelector
            )
            where T : struct
            where TAccumulate : struct
            where TResult : struct
            where TFunc : struct, IFunc<TAccumulate, T, TAccumulate>
            where TResultSelector : struct, IFunc<TAccumulate, TResult>
        {
            var aggregateFunc = source.AggregateAsFunc(seed, func, resultSelector);
            return source.Run(aggregateFunc);
        }

        public static JobHandle<TResult> ScheduleAggregate<T, TAccumulate, TResult, TFunc, TResultSelector>(
            this in NativeArray<T> source,
            TAccumulate seed,
            ValueFunc<TAccumulate, T, TAccumulate>.Struct<TFunc> func,
            ValueFunc<TAccumulate, TResult>.Struct<TResultSelector> resultSelector
            )
            where T : struct
            where TAccumulate : struct
            where TResult : struct
            where TFunc : struct, IFunc<TAccumulate, T, TAccumulate>
            where TResultSelector : struct, IFunc<TAccumulate, TResult>
        {
            var aggregateFunc = source.AggregateAsFunc(seed, func, resultSelector);
            return source.Schedule(aggregateFunc);
        }

        public static JobHandle ScheduleAggregate<T, TAccumulate, TResult, TFunc, TResultSelector>(
            this in NativeArray<T> source,
            ref NativeArray<TResult> output,
            TAccumulate seed,
            ValueFunc<TAccumulate, T, TAccumulate>.Struct<TFunc> func,
            ValueFunc<TAccumulate, TResult>.Struct<TResultSelector> resultSelector
            )
            where T : struct
            where TAccumulate : struct
            where TResult : struct
            where TFunc : struct, IFunc<TAccumulate, T, TAccumulate>
            where TResultSelector : struct, IFunc<TAccumulate, TResult>
        {
            var aggregateFunc = source.AggregateAsFunc(seed, func, resultSelector);
            return source.Schedule(aggregateFunc, ref output);
        }

        public static TAccumulate Aggregate<T, TAccumulate, TFunc>(
            this in NativeArray<T> source,
            TAccumulate seed,
            ValueFunc<TAccumulate, T, TAccumulate>.Struct<TFunc> func
            )
            where T : struct
            where TAccumulate : struct
            where TFunc : struct, IFunc<TAccumulate, T, TAccumulate>
        {
            for (var i = 0; i < source.Length; i++)
                seed = func.Invoke(seed, source[i]);
            return seed;
        }

        public struct ArrayAggregateFunc<T, TAccumulate, TFunc>
            : IFunc<NativeArray<T>, TAccumulate>
            where T : struct
            where TAccumulate : struct
            where TFunc : struct, IFunc<TAccumulate, T, TAccumulate>
        {
            public TAccumulate Seed;
            public ValueFunc<TAccumulate, T, TAccumulate>.Struct<TFunc> Func;

            public TAccumulate Invoke(NativeArray<T> seq)
            {
                return seq.Aggregate(Seed, Func);
            }
        }

        public static ValueFunc<NativeArray<T>, TAccumulate>.Struct<ArrayAggregateFunc<T, TAccumulate, TFunc>>
        AggregateAsFunc<T, TAccumulate, TFunc>(
            this in NativeArray<T> source,
            TAccumulate seed,
            ValueFunc<TAccumulate, T, TAccumulate>.Struct<TFunc> func
            )
            where T : struct
            where TAccumulate : struct
            where TFunc : struct, IFunc<TAccumulate, T, TAccumulate>
        {
            var seqAggregateFunc = new ArrayAggregateFunc<T, TAccumulate, TFunc> { Seed = seed, Func = func };
            return ValueFunc<NativeArray<T>, TAccumulate>.New(seqAggregateFunc);
        }

        public static TAccumulate RunAggregate<T, TAccumulate, TFunc>(
            this in NativeArray<T> source,
            TAccumulate seed,
            ValueFunc<TAccumulate, T, TAccumulate>.Struct<TFunc> func
            )
            where T : struct
            where TAccumulate : struct
            where TFunc : struct, IFunc<TAccumulate, T, TAccumulate>
        {
            var aggregateFunc = source.AggregateAsFunc(seed, func);
            return source.Run(aggregateFunc);
        }

        public static JobHandle<TAccumulate> ScheduleAggregate<T, TAccumulate, TFunc>(
            this in NativeArray<T> source,
            TAccumulate seed,
            ValueFunc<TAccumulate, T, TAccumulate>.Struct<TFunc> func
            )
            where T : struct
            where TAccumulate : struct
            where TFunc : struct, IFunc<TAccumulate, T, TAccumulate>
        {
            var aggregateFunc = source.AggregateAsFunc(seed, func);
            return source.Schedule(aggregateFunc);
        }

        public static JobHandle ScheduleAggregate<T, TAccumulate, TFunc>(
            this in NativeArray<T> source,
            ref NativeArray<TAccumulate> output,
            TAccumulate seed,
            ValueFunc<TAccumulate, T, TAccumulate>.Struct<TFunc> func
            )
            where T : struct
            where TAccumulate : struct
            where TFunc : struct, IFunc<TAccumulate, T, TAccumulate>
        {
            var aggregateFunc = source.AggregateAsFunc(seed, func);
            return source.Schedule(aggregateFunc, ref output);
        }

        public static T Aggregate<T, TFunc>(
            this in NativeArray<T> source,
            ValueFunc<T, T, T>.Struct<TFunc> func
            )
            where T : struct
            where TFunc : struct, IFunc<T, T, T>
        {
            if (source.Length == 0) throw Error.NoElements();
            var seed = source[0];
            for (var i = 1; i < source.Length; i++)
                seed = func.Invoke(seed, source[i]);
            return seed;
        }

        public struct ArrayAggregateFunc<T, TFunc>
            : IFunc<NativeArray<T>, T>
            where T : struct
            where TFunc : struct, IFunc<T, T, T>
        {
            public ValueFunc<T, T, T>.Struct<TFunc> Func;

            public T Invoke(NativeArray<T> seq)
            {
                return seq.Aggregate(Func);
            }
        }

        public static ValueFunc<NativeArray<T>, T>.Struct<ArrayAggregateFunc<T, TFunc>>
        AggregateAsFunc<T, TFunc>(
            this in NativeArray<T> source,
            ValueFunc<T, T, T>.Struct<TFunc> func
            )
            where T : struct
            where TFunc : struct, IFunc<T, T, T>
        {
            var seqAggregateFunc = new ArrayAggregateFunc<T, TFunc> { Func = func };
            return ValueFunc<NativeArray<T>, T>.New(seqAggregateFunc);
        }

        public static T RunAggregate<T, TFunc>(
            this in NativeArray<T> source,
            ValueFunc<T, T, T>.Struct<TFunc> func
            )
            where T : struct
            where TFunc : struct, IFunc<T, T, T>
        {
            var aggregateFunc = source.AggregateAsFunc(func);
            return source.Run(aggregateFunc);
        }

        public static JobHandle<T> ScheduleAggregate<T, TFunc>(
            this in NativeArray<T> source,
            ValueFunc<T, T, T>.Struct<TFunc> func
            )
            where T : struct
            where TFunc : struct, IFunc<T, T, T>
        {
            var aggregateFunc = source.AggregateAsFunc(func);
            return source.Schedule(aggregateFunc);
        }

        public static JobHandle ScheduleAggregate<T, TFunc>(
            this in NativeArray<T> source,
            ref NativeArray<T> output,
            ValueFunc<T, T, T>.Struct<TFunc> func
            )
            where T : struct
            where TFunc : struct, IFunc<T, T, T>
        {
            var aggregateFunc = source.AggregateAsFunc(func);
            return source.Schedule(aggregateFunc, ref output);
        }
    }
}
