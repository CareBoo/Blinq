using System;
using System.Collections;
using NUnit.Framework;

[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true, Inherited = false)]
internal class ArrayValuesAttribute : ValueSourceAttribute
{
    public ArrayValuesAttribute() :
        base(typeof(ArrayValues), nameof(ArrayValues.Values))
    { }
}

internal class ArrayValues
{
    public static IEnumerable Values
    {
        get
        {
            yield return Range(0, 10);
            yield return Range(0, 0);
        }
    }

    private static int[] Range(int start, int count)
    {
        var arr = new int[count];
        for (var i = 0; i < count; i++)
        {
            arr[i] = start + i;
        }
        return arr;
    }
}
