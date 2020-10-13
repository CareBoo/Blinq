using System;
using System.Collections.Generic;
using NUnit.Framework;

internal static class Utils
{
    public static (Exception exception, T value) ExceptionAndValue<T>(Func<T> func)
    {
        Exception exception = null;
        T value = default;
        try
        {
            value = func.Invoke();
        }
        catch (Exception ex)
        {
            exception = ex;
        }
        return (exception, value);
    }

    public static void AssertAreEqual<T>(
        (Exception, T) expected,
        (Exception, T) actual
        )
    {
        if (expected.Item1 != null)
            Assert.IsNotNull(actual.Item1);
        else
            Assert.IsNull(actual.Item1);
        Assert.AreEqual(expected.Item2, actual.Item2);
    }

    public static string LogEnumerables(IEnumerable<int> expected, IEnumerable<int> actual)
    {
        return $"Expected: {{{string.Join(",", expected)}}}\nActual: {{{string.Join(",", actual)}}}";
    }
}
