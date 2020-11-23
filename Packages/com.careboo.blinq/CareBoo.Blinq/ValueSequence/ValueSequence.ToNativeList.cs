using Unity.Collections;
using Unity.Jobs;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public struct ToNativeListJob<T, TSource> : IJob
            where T : struct
            where TSource : struct, ISequence<T>
        {
            [ReadOnly]
            public ValueSequence<T, TSource> Source;

            [WriteOnly]
            public NativeList<T> Output;

            public void Execute()
            {
                var list = Source.ToNativeList(Allocator.Temp);
                Output.Capacity = list.Length;
                NativeArray<T>.Copy(list, Output);
                list.Dispose();
            }
        }

        public static NativeList<T> RunToNativeList<T, TSource>(
            this in ValueSequence<T, TSource> source,
            in Allocator allocator
            )
            where T : struct
            where TSource : struct, ISequence<T>
        {
            var output = new NativeList<T>(allocator);
            new ToNativeListJob<T, TSource> { Source = source, Output = output }.Run();
            return output;
        }

        public static JobHandle ScheduleToNativeList<T, TSource>(
            this in ValueSequence<T, TSource> source,
            ref NativeList<T> output
            )
            where T : struct
            where TSource : struct, ISequence<T>
        {
            return new ToNativeListJob<T, TSource> { Source = source, Output = output }.Schedule();
        }

        public static CollectionJobHandle<NativeList<T>> ScheduleToNativeList<T, TSource>(
            this in ValueSequence<T, TSource> source,
            in Allocator allocator
            )
            where T : struct
            where TSource : struct, ISequence<T>
        {
            var output = new NativeList<T>(allocator);
            var jobHandle = new ToNativeListJob<T, TSource> { Source = source, Output = output }.Schedule();
            return new CollectionJobHandle<NativeList<T>>(in jobHandle, in output);
        }
    }
}
