using System;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace CareBoo.Blinq
{
    [BurstCompile(CompileSynchronously = true)]
    public struct AggregateJob<TSource, TAccumulate, TAccumulator> : IJob
        where TSource : unmanaged
        where TAccumulate : unmanaged
        where TAccumulator : IAccumulator<TSource, TAccumulate>
    {
        [ReadOnly]
        public NativeArray<TSource> Input;

        public TAccumulate Seed;

        [ReadOnly]
        public TAccumulator Accumulator;

        [WriteOnly]
        public NativeArray<TAccumulate> Output;

        public void Execute()
        {
            for (var i = 0; i < Input.Length; i++)
                Seed = Accumulator.Invoke(Input[i], Seed);
            Output[0] = Seed;
        }
    }

    [BurstCompile(CompileSynchronously = true)]
    public struct AggregateJob<TSource, TAccumulate, TResult, TAccumulator, TResultSelector> : IJob
        where TSource : unmanaged
        where TAccumulate : unmanaged
        where TResult : unmanaged
        where TAccumulator : IAccumulator<TSource, TAccumulate>
        where TResultSelector : IFunc<TAccumulate, TResult>
    {
        [ReadOnly]
        public NativeArray<TSource> Input;

        public TAccumulate Seed;

        [ReadOnly]
        public TAccumulator Accumulator;

        [ReadOnly]
        public TResultSelector ResultSelector;

        [WriteOnly]
        public NativeArray<TResult> Output;

        public void Execute()
        {
            for (var i = 0; i < Input.Length; i++)
                Seed = Accumulator.Invoke(Input[i], Seed);
            Output[0] = ResultSelector.Invoke(Seed);
        }
    }

    public static partial class BlinqExtensions
    {
        public static TSource Aggregate<TSource, TAccumulator>(this ref NativeArray<TSource> source, JobHandle dependsOn = default)
            where TSource : unmanaged
            where TAccumulator : IAccumulator<TSource, TSource>
        {
            return source.Aggregate<TSource, TSource, TAccumulator>(dependsOn);
        }

        public static TAccumulate Aggregate<TSource, TAccumulate, TAccumulator>(this ref NativeArray<TSource> source, JobHandle dependsOn = default)
            where TSource : unmanaged
            where TAccumulate : unmanaged
            where TAccumulator : IAccumulator<TSource, TAccumulate>
        {
            var output = new NativeArray<TAccumulate>(1, Allocator.Persistent);
            var job = new AggregateJob<TSource, TAccumulate, TAccumulator> { Input = source, Accumulator = default, Output = output };
            job.Schedule(dependsOn).Complete();
            var result = output[0];
            output.Dispose();
            return result;
        }

        public static TResult Aggregate<TSource, TResult, TAccumulator, TResultSelector>(this ref NativeArray<TSource> source, JobHandle dependsOn = default)
            where TSource : unmanaged
            where TResult : unmanaged
            where TAccumulator : IAccumulator<TSource, TSource>
            where TResultSelector : IFunc<TSource, TResult>
        {
            return source.Aggregate<TSource, TSource, TResult, TAccumulator, TResultSelector>(dependsOn);
        }

        public static TResult Aggregate<TSource, TAccumulate, TResult, TAccumulator, TResultSelector>(this ref NativeArray<TSource> source, JobHandle dependsOn = default)
            where TSource : unmanaged
            where TAccumulate : unmanaged
            where TResult : unmanaged
            where TAccumulator : IAccumulator<TSource, TAccumulate>
            where TResultSelector : IFunc<TAccumulate, TResult>
        {
            var output = new NativeArray<TResult>(1, Allocator.Persistent);
            var job = new AggregateJob<TSource, TAccumulate, TResult, TAccumulator, TResultSelector> { Input = source, Accumulator = default, ResultSelector = default, Output = output };
            job.Schedule(dependsOn).Complete();
            var result = output[0];
            output.Dispose();
            return result;
        }

        public static TSource Aggregate<TSource, TAccumulator>(this TSource[] source)
            where TSource : unmanaged
            where TAccumulator : IAccumulator<TSource, TSource>
        {
            return source.Aggregate<TSource, TSource, TAccumulator>();
        }

        public static TAccumulate Aggregate<TSource, TAccumulate, TAccumulator>(this TSource[] source)
            where TSource : unmanaged
            where TAccumulate : unmanaged
            where TAccumulator : IAccumulator<TSource, TAccumulate>
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var input = new NativeArray<TSource>(source, Allocator.Persistent);
            var result = input.Aggregate<TSource, TAccumulate, TAccumulator>();
            input.Dispose();
            return result;
        }

        public static TResult Aggregate<TSource, TResult, TAccumulator, TResultSelector>(this TSource[] source)
            where TSource : unmanaged
            where TResult : unmanaged
            where TAccumulator : IAccumulator<TSource, TSource>
            where TResultSelector : IFunc<TSource, TResult>
        {
            return source.Aggregate<TSource, TSource, TResult, TAccumulator, TResultSelector>();
        }

        public static TResult Aggregate<TSource, TAccumulate, TResult, TAccumulator, TResultSelector>(this TSource[] source)
            where TSource : unmanaged
            where TAccumulate : unmanaged
            where TResult : unmanaged
            where TAccumulator : IAccumulator<TSource, TAccumulate>
            where TResultSelector : IFunc<TAccumulate, TResult>
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var input = new NativeArray<TSource>(source, Allocator.Persistent);
            var result = input.Aggregate<TSource, TAccumulate, TResult, TAccumulator, TResultSelector>();
            input.Dispose();
            return result;
        }

        public static TSource Aggregate<TSource, TAccumulator>(this List<TSource> source)
            where TSource : unmanaged
            where TAccumulator : IAccumulator<TSource, TSource>
        {
            return source.Aggregate<TSource, TSource, TAccumulator>();
        }

        public static TAccumulate Aggregate<TSource, TAccumulate, TAccumulator>(this List<TSource> source)
            where TSource : unmanaged
            where TAccumulate : unmanaged
            where TAccumulator : IAccumulator<TSource, TAccumulate>
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var input = source.ToNativeArray(Allocator.Persistent);
            var result = input.Aggregate<TSource, TAccumulate, TAccumulator>();
            input.Dispose();
            return result;
        }

        public static TResult Aggregate<TSource, TResult, TAccumulator, TResultSelector>(this List<TSource> source)
            where TSource : unmanaged
            where TResult : unmanaged
            where TAccumulator : IAccumulator<TSource, TSource>
            where TResultSelector : IFunc<TSource, TResult>
        {
            return source.Aggregate<TSource, TSource, TResult, TAccumulator, TResultSelector>();
        }

        public static TResult Aggregate<TSource, TAccumulate, TResult, TAccumulator, TResultSelector>(this List<TSource> source)
            where TSource : unmanaged
            where TAccumulate : unmanaged
            where TResult : unmanaged
            where TAccumulator : IAccumulator<TSource, TAccumulate>
            where TResultSelector : IFunc<TAccumulate, TResult>
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var input = source.ToNativeArray(Allocator.Persistent);
            var result = input.Aggregate<TSource, TAccumulate, TResult, TAccumulator, TResultSelector>();
            input.Dispose();
            return result;
        }
    }
}
