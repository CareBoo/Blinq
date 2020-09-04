using System;
using System.Collections;
using NUnit.Framework;
using Unity.Collections;

[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true, Inherited = false)]
internal class NativeArrayValuesAttribute : ValueSourceAttribute
{
    public NativeArrayValuesAttribute() :
        base(typeof(NativeArrayValues), nameof(NativeArrayValues.Values))
    { }
}

internal class NativeArrayValues
{
    public static IEnumerable Values
    {
        get
        {
            yield return Range(0, 10);
            yield return Range(0, 0);
        }
    }

    private static NativeArray<int> Range(int start, int count)
    {
        var arr = new NativeArray<int>(count, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
        for (var i = 0; i < count; i++)
        {
            arr[i] = start + i;
        }
        return arr;
    }
}
