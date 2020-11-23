using Unity.Collections;
using Unity.Jobs;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static NativeList<T> ToNativeList<T>(
            this in NativeArray<T> source,
            in Allocator allocator
            )
            where T : struct
        {
            var result = new NativeList<T>(source.Length, allocator);
            result.CopyFrom(source);
            return result;
        }

        public static NativeList<T> RunToNativeList<T>(
            this in NativeArray<T> source,
            in Allocator allocator
            )
            where T : struct
        {
            return source.ToValueSequence().RunToNativeList(allocator);
        }

        public static JobHandle ScheduleToNativeList<T>(
            this in NativeArray<T> source,
            ref NativeList<T> output
            )
            where T : struct
        {
            return source.ToValueSequence().ScheduleToNativeList(ref output);
        }

        public static CollectionJobHandle<NativeList<T>> ScheduleToNativeList<T>(
            this in NativeArray<T> source,
            Allocator allocator
            )
            where T : struct
        {
            return source.ToValueSequence().ScheduleToNativeList(allocator);
        }
    }
}
