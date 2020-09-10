using System;
using System.Collections.Generic;
using NUnit.Framework;

internal static class Utils
{
    public static (string exceptionMessage, T value) ExceptionAndValue<T>(Func<T> func)
    {
        string exceptionMessage = null;
        T value = default;
        try
        {
            value = func.Invoke();
        }
        catch (Exception ex)
        {
            exceptionMessage = ex.Message;
        }
        return (exceptionMessage, value);
    }

    public static void AssertAreEqual<T>(
        (string, T) expected,
        (string, T) actual
        )
    {
        Assert.AreEqual(expected.Item1, actual.Item1);
        Assert.AreEqual(expected.Item2, actual.Item2);
    }

    public static string LogEnumerables(IEnumerable<int> expected, IEnumerable<int> actual)
    {
        return $"Expected: {{{string.Join(",", expected)}}}\nActual: {{{string.Join(",", actual)}}}";
    }
}
