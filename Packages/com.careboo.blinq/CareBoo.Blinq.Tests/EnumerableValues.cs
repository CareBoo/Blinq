using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using LinqEnumerable = System.Linq.Enumerable;

[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true, Inherited = false)]
internal class EnumerableValuesAttribute : ValueSourceAttribute
{
    public EnumerableValuesAttribute() :
        base(typeof(EnumerableValues), nameof(EnumerableValues.Values))
    { }
}

internal class EnumerableValues
{
    public static IEnumerable Values
    {
        get
        {
            yield return Range(0, 10);
            yield return Range(0, 0);
        }
    }

    private static IEnumerable<int> Range(int start, int count)
    {
        // For some reason, we need to convert to an array
        // so that the tests can recognize the type parameter.
        return LinqEnumerable.ToArray(LinqEnumerable.Range(start, count));
    }
}
