using Unity.Collections;
using Unity.Jobs;

namespace CareBoo.Blinq
{
    public static partial class NativeSequenceExtensions
    {
        public static double Average(this NativeSequence<int> source)
        {
            source.DependsOn.Complete();
            var length = source.Length;
            if (length == 0) throw Error.NoElements();

            var output = new NativeArray<long>(1, Allocator.Persistent, NativeArrayOptions.ClearMemory);
            var job = new SumJob_int { Input = source, Output = output };
            job.Schedule(length, 64).Complete();
            var result = (double)output[0] / length;

            output.Dispose();
            return result;
        }

        public static double Average<TSource>(this NativeSequence<TSource> source, BurstCompiledFunc<TSource, int> selector)
            where TSource : struct
        {
            source.DependsOn.Complete();
            var length = source.Length;
            if (length == 0) throw Error.NoElements();

            var output = new NativeArray<long>(1, Allocator.Persistent, NativeArrayOptions.ClearMemory);
            var job = new SumJob_int<TSource> { Input = source, Selector = selector, Output = output };
            job.Schedule(length, 64).Complete();
            var result = (double)output[0] / length;

            output.Dispose();
            return result;
        }

        public struct SumJob_int : IJobParallelFor
        {
            [ReadOnly]
            [DeallocateOnJobCompletion]
            public NativeArray<int> Input;

            [NativeDisableParallelForRestriction]
            public NativeArray<long> Output;

            public void Execute(int index)
            {
                Output[0] += Input[index];
            }
        }

        public struct SumJob_int<TSource> : IJobParallelFor
            where TSource : struct
        {
            [ReadOnly]
            [DeallocateOnJobCompletion]
            public NativeArray<TSource> Input;

            [ReadOnly]
            public BurstCompiledFunc<TSource, int> Selector;

            [NativeDisableParallelForRestriction]
            public NativeArray<long> Output;

            public void Execute(int index)
            {
                Output[0] += Selector.Invoke(Input[index]);
            }
        }
    }
}
