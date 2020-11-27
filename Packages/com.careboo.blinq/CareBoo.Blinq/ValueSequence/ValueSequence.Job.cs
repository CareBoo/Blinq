using System;
using System.Collections.Generic;
using CareBoo.Burst.Delegates;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static Job<T, TSource, TSourceEnumerator, TResult, TResultSelector> NewJob<T, TSource, TSourceEnumerator, TResult, TResultSelector>(
            this in ValueSequence<T, TSource, TSourceEnumerator> seq,
            ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, TResult>.Struct<TResultSelector> resultSelector,
            ref NativeArray<TResult> output
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TResult : struct
            where TResultSelector : struct, IFunc<ValueSequence<T, TSource, TSourceEnumerator>, TResult>
        {
            return new Job<T, TSource, TSourceEnumerator, TResult, TResultSelector>(in seq, resultSelector, ref output);
        }

        public static TResult Run<T, TSource, TSourceEnumerator, TResult, TResultSelector>(
            this in ValueSequence<T, TSource, TSourceEnumerator> seq,
            ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, TResult>.Struct<TResultSelector> resultSelector
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TResult : struct
            where TResultSelector : struct, IFunc<ValueSequence<T, TSource, TSourceEnumerator>, TResult>
        {
            var output = new NativeArray<TResult>(1, Allocator.Persistent);
            NewJob(in seq, resultSelector, ref output).Run();
            var result = output[0];
            output.Dispose();
            return result;
        }

        public static JobHandle Schedule<T, TSource, TSourceEnumerator, TResult, TResultSelector>(
            this in ValueSequence<T, TSource, TSourceEnumerator> seq,
            ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, TResult>.Struct<TResultSelector> resultSelector,
            ref NativeArray<TResult> output
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TResult : struct
            where TResultSelector : struct, IFunc<ValueSequence<T, TSource, TSourceEnumerator>, TResult>
        {
            return NewJob(in seq, resultSelector, ref output).Schedule();
        }

        public static JobHandle<TResult> Schedule<T, TSource, TSourceEnumerator, TResult, TResultSelector>(
            this in ValueSequence<T, TSource, TSourceEnumerator> seq,
            ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, TResult>.Struct<TResultSelector> resultSelector
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TResult : struct
            where TResultSelector : struct, IFunc<ValueSequence<T, TSource, TSourceEnumerator>, TResult>
        {
            var output = new NativeArray<TResult>(1, Allocator.Persistent);
            var jobHandle = NewJob(in seq, resultSelector, ref output).Schedule();
            var seqJobHandle = new JobHandle<TResult>(in jobHandle, ref output);
            return seqJobHandle;
        }

        [BurstCompile]
        public struct Job<T, TSource, TSourceEnumerator, TResult, TResultSelector> : IJob
        where T : struct
        where TSource : struct, ISequence<T, TSourceEnumerator>
        where TSourceEnumerator : struct, IEnumerator<T>
        where TResult : struct
        where TResultSelector : struct, IFunc<ValueSequence<T, TSource, TSourceEnumerator>, TResult>
        {
            [ReadOnly]
            readonly ValueSequence<T, TSource, TSourceEnumerator> seq;

            [ReadOnly]
            readonly ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, TResult>.Struct<TResultSelector> resultSelector;

            [WriteOnly]
            NativeArray<TResult> output;

            internal Job(
                in ValueSequence<T, TSource, TSourceEnumerator> seq,
                ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, TResult>.Struct<TResultSelector> resultSelector,
                ref NativeArray<TResult> output
                )
            {
                this.seq = seq;
                this.resultSelector = resultSelector;
                this.output = output;
            }

            public void Execute()
            {
                output[0] = resultSelector.Invoke(seq);
            }
        }

        public struct JobHandle<TResult> : IDisposable
            where TResult : struct
        {
            JobHandle jobHandle;
            NativeArray<TResult> output;

            public bool IsCompleted => jobHandle.IsCompleted;

            internal JobHandle(in JobHandle jobHandle, ref NativeArray<TResult> output)
            {
                this.jobHandle = jobHandle;
                this.output = output;
            }

            public TResult Complete()
            {
                jobHandle.Complete();
                var result = output[0];
                Dispose();
                return result;
            }

            public void Dispose()
            {
                if (output.IsCreated)
                    output.Dispose();
            }
        }
    }
}
