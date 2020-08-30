namespace CareBoo.Blinq
{
    public interface IFunc<out TResult>
        where TResult : struct
    {
        TResult Invoke();
    }

    public interface IFunc<T, out TResult>
        where T : struct
        where TResult : struct
    {
        TResult Invoke(T arg0);
    }

    public interface IFunc<T, U, out TResult>
        where T : struct
        where U : struct
        where TResult : struct
    {
        TResult Invoke(T arg0, U arg1);
    }

    public interface IFunc<T, U, V, out TResult>
        where T : struct
        where U : struct
        where V : struct
        where TResult : struct
    {
        TResult Invoke(T arg0, U arg1, V arg2);
    }

    public interface IFunc<T, U, V, W, out TResult>
        where T : struct
        where U : struct
        where V : struct
        where W : struct
        where TResult : struct
    {
        TResult Invoke(T arg0, U arg1, V arg2, W arg3);
    }
}
