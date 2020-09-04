using System;
using System.Collections.Generic;

internal static class Utils
{
    public static (string exceptionMessage, T value) ExceptionOrValue<T>(Func<T> func)
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

    public static string LogEnumerables(IEnumerable<int> expected, IEnumerable<int> actual)
    {
        return $"Expected: {{{string.Join(",", expected)}}}\nActual: {{{string.Join(",", actual)}}}";
    }
}
