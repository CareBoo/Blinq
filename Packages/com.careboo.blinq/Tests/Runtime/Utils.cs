using System;

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
}
