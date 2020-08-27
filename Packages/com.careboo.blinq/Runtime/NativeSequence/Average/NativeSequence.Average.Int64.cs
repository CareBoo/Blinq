using Unity.Collections;
using Unity.Jobs;

namespace CareBoo.Blinq
{
    public static partial class NativeSequenceExtensions
    {
        public static double Average(this NativeSequence<long> source)
        {
            if (source.Length == 0) throw Error.NoElements();

            var output = new NativeArray<long>(1, Allocator.Persistent, NativeArrayOptions.ClearMemory);
            var job = new SumJob_long { Input = source, Output = output };
            job.Schedule(source.Length, 64, source.DependsOn).Complete();
            var result = (double)output[0] / source.Length;

            output.Dispose();
            return result;
        }

        public static double Average<TSource>(this NativeSequence<TSource> source, BurstCompiledFunc<TSource, long> selector)
            where TSource : struct
        {
            if (source.Length == 0) throw Error.NoElements();

            var output = new NativeArray<long>(1, Allocator.Persistent, NativeArrayOptions.ClearMemory);
            var job = new SumJob_long<TSource> { Input = source, Selector = selector, Output = output };
            job.Schedule(source.Length, 64, source.DependsOn).Complete();
            var result = (double)output[0] / source.Length;

            output.Dispose();
            return result;
        }

        public struct SumJob_long : IJobParallelFor
        {
            [ReadOnly]
            [DeallocateOnJobCompletion]
            public NativeArray<long> Input;

            [NativeDisableParallelForRestriction]
            public NativeArray<long> Output;

            public void Execute(int index)
            {
                Output[0] += Input[index];
            }
        }

        public struct SumJob_long<TSource> : IJobParallelFor
            where TSource : struct
        {
            [ReadOnly]
            [DeallocateOnJobCompletion]
            public NativeArray<TSource> Input;

            [ReadOnly]
            public BurstCompiledFunc<TSource, long> Selector;

            [NativeDisableParallelForRestriction]
            public NativeArray<long> Output;

            public void Execute(int index)
            {
                Output[0] += Selector.Invoke(Input[index]);
            }
        }
    }
}
