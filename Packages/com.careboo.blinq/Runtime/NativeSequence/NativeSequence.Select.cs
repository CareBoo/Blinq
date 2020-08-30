using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace CareBoo.Blinq
{
    public partial struct NativeSequence<T>
        where T : struct
    {
        [CodeGenSourceApi("02618425-8d2b-4858-be01-7dfb335b15b5")]
        public NativeSequence<TResult> Select<TResult>(Func<T, int, TResult> selector)
            where TResult : struct
        {
            throw Error.NotCodeGenerated();
        }

        [CodeGenTargetApi("02618425-8d2b-4858-be01-7dfb335b15b5")]
        public NativeSequence<TResult> SelectWithIndex<TResult, TSelector>(TSelector selector)
            where TResult : struct
            where TSelector : struct, IFunc<T, int, TResult>
        {
            var output = new NativeArray<TResult>(input.Length, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
            var job = new SelectWithIndexJob<TResult, TSelector> { Input = input, Selector = selector, Output = output };
            return new NativeSequence<TResult>(
                output,
                job.Schedule(input.Length, 32, dependsOn)
            );
        }

        [CodeGenSourceApi("cd64809b-cb18-434e-8b22-81f4a133c657")]
        public NativeSequence<TResult> Select<TResult>(Func<T, TResult> selector)
            where TResult : struct
        {
            throw Error.NotCodeGenerated();
        }

        [CodeGenTargetApi("cd64809b-cb18-434e-8b22-81f4a133c657")]
        public NativeSequence<TResult> Select<TResult, TSelector>(TSelector selector)
            where TResult : struct
            where TSelector : struct, IFunc<T, TResult>
        {
            var output = new NativeArray<TResult>(input.Length, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
            var job = new SelectJob<TResult, TSelector> { Input = input, Selector = selector, Output = output };
            return new NativeSequence<TResult>(
                output,
                job.Schedule(input.Length, 32, dependsOn)
            );
        }

        [BurstCompile(CompileSynchronously = true)]
        public struct SelectWithIndexJob<TResult, TSelector> : IJobParallelFor
            where TResult : struct
            where TSelector : struct, IFunc<T, int, TResult>
        {
            [ReadOnly]
            [DeallocateOnJobCompletion]
            public NativeArray<T> Input;

            [ReadOnly]
            public TSelector Selector;

            [WriteOnly]
            public NativeArray<TResult> Output;

            public void Execute(int index)
            {
                Output[index] = Selector.Invoke(Input[index], index);
            }
        }

        [BurstCompile(CompileSynchronously = true)]
        public struct SelectJob<TResult, TSelector> : IJobParallelFor
            where TResult : struct
            where TSelector : struct, IFunc<T, TResult>
        {
            [ReadOnly]
            [DeallocateOnJobCompletion]
            public NativeArray<T> Input;

            [ReadOnly]
            public TSelector Selector;

            [WriteOnly]
            public NativeArray<TResult> Output;

            public void Execute(int index)
            {
                Output[index] = Selector.Invoke(Input[index]);
            }
        }
    }
}
