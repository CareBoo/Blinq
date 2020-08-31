using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

[BurstCompile(CompileSynchronously = true)]
internal struct CopyJob<T> : IJobParallelFor
    where T : struct
{
    [ReadOnly]
    public NativeArray<T> Input;

    [WriteOnly]
    public NativeArray<T> Output;

    public void Execute(int index)
    {
        Output[index] = Input[index];
    }
}

internal static class NativeListUtility
{
    public static NativeList<T> ToNativeList<T>(this ref NativeArray<T> input, Allocator allocator)
        where T : struct
    {
        var output = new NativeList<T>(input.Length, allocator) { Length = input.Length };
        var copyJob = new CopyJob<T> { Input = input, Output = output };
        copyJob.Schedule(input.Length, 64).Complete();
        return output;
    }

    public static NativeList<T> ToNativeList<T>(this T[] input, Allocator allocator)
        where T : struct
    {
        var nativeArrayInput = new NativeArray<T>(input, Allocator.Persistent);
        var result = nativeArrayInput.ToNativeList(allocator);
        nativeArrayInput.Dispose();
        return result;
    }

    public static NativeList<T> ToNativeList<T>(this List<T> input, Allocator allocator)
        where T : struct
    {
        return input.ToArray().ToNativeList(allocator);
    }
}
