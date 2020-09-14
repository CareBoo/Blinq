using System;
using System.Collections;
using System.Linq;
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
            yield return Enumerable.Range(0, 10).ToArray();
            yield return Enumerable.Range(0, 0).ToArray();
            yield return Enumerable.Range(0, 10).Concat(Enumerable.Range(0, 10)).ToArray();
        }
    }
}
