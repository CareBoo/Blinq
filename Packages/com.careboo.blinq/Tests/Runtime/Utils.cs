using System;
using System.Collections.Generic;
using CareBoo.Blinq;
using Unity.Collections;
using LinqEnumerable = System.Linq.Enumerable;

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

    public static NativeSequence<T> InitSequence<T>(IEnumerable<T> source)
        where T : struct
    {
        return new NativeSequence<T>(LinqEnumerable.ToArray(source), Allocator.Persistent);
    }
}
