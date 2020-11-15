using System.Collections.Generic;
using Unity.Collections;
using LinqEnumerable = System.Linq.Enumerable;

internal class EnumerableValues
{
    public const int NumElements = 16 << 10;

    public static int[] GetZeroedIntArray()
    {
        return new int[NumElements];
    }

    public static List<int> GetZeroedIntList()
    {
        return LinqEnumerable.ToList(GetZeroedIntArray());
    }

    public static NativeArray<int> GetZeroedIntNativeArray(Allocator allocator)
    {
        return new NativeArray<int>(GetZeroedIntArray(), allocator);
    }
}
