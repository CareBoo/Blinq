using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        [BurstCompile]
        public struct ToNativeListJob<T, TSource, TSourceEnumerator> : IJob
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            [ReadOnly]
            public ValueSequence<T, TSource, TSourceEnumerator> Source;

            [WriteOnly]
            public NativeList<T> Output;

            public void Execute()
            {
                var list = Source.ToNativeList(Allocator.Temp);
                Output.Capacity = list.Length;
                Output.AddRangeNoResize(list);
                list.Dispose();
            }
        }

        public static NativeList<T> RunToNativeList<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            Allocator allocator
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var output = new NativeList<T>(allocator);
            new ToNativeListJob<T, TSource, TSourceEnumerator> { Source = source, Output = output }.Run();
            return output;
        }

        public static JobHandle ScheduleToNativeList<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ref NativeList<T> output
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            return new ToNativeListJob<T, TSource, TSourceEnumerator> { Source = source, Output = output }.Schedule();
        }

        public static CollectionJobHandle<NativeList<T>> ScheduleToNativeList<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            Allocator allocator
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var output = new NativeList<T>(allocator);
            var jobHandle = new ToNativeListJob<T, TSource, TSourceEnumerator> { Source = source, Output = output }.Schedule();
            return new CollectionJobHandle<NativeList<T>>(in jobHandle, in output);
        }
    }
}
