using Unity.Collections;
using Unity.Jobs;

namespace CareBoo.Blinq
{
    public static partial class NativeSequenceExtensions
    {
        public static decimal Average(this NativeSequence<decimal> source)
        {
            source.DependsOn.Complete();
            var length = source.Length;
            if (length == 0) throw Error.NoElements();

            var output = new NativeArray<decimal>(1, Allocator.Persistent, NativeArrayOptions.ClearMemory);
            var job = new SumJob_decimal { Input = source, Output = output };
            job.Schedule(length, 64).Complete();
            var result = output[0] / length;

            output.Dispose();
            return result;
        }

        public static decimal Average<TSource>(this NativeSequence<TSource> source, BurstCompiledFunc<TSource, decimal> selector)
            where TSource : struct
        {
            source.DependsOn.Complete();
            var length = source.Length;
            if (length == 0) throw Error.NoElements();

            var output = new NativeArray<decimal>(1, Allocator.Persistent, NativeArrayOptions.ClearMemory);
            var job = new SumJob_decimal<TSource> { Input = source, Selector = selector, Output = output };
            job.Schedule(length, 64).Complete();
            var result = output[0] / length;

            output.Dispose();
            return result;
        }

        public struct SumJob_decimal : IJobParallelFor
        {
            [ReadOnly]
            [DeallocateOnJobCompletion]
            public NativeArray<decimal> Input;

            [NativeDisableParallelForRestriction]
            public NativeArray<decimal> Output;

            public void Execute(int index)
            {
                Output[0] += Input[index];
            }
        }

        public struct SumJob_decimal<TSource> : IJobParallelFor
            where TSource : struct
        {
            [ReadOnly]
            [DeallocateOnJobCompletion]
            public NativeArray<TSource> Input;

            [ReadOnly]
            public BurstCompiledFunc<TSource, decimal> Selector;

            [NativeDisableParallelForRestriction]
            public NativeArray<decimal> Output;

            public void Execute(int index)
            {
                Output[0] += Selector.Invoke(Input[index]);
            }
        }
    }
}
