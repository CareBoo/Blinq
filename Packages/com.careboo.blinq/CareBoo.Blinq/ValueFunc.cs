using Unity.Burst;

namespace CareBoo.Blinq
{
    public interface IValueFunc<out TResult>
        where TResult : struct
    {
        TResult Invoke();
    }

    public interface IValueFunc<in T, out TResult>
        where T : struct
        where TResult : struct
    {
        TResult Invoke(T arg0);
    }

    public interface IValueFunc<in T, in U, out TResult>
        where T : struct
        where U : struct
        where TResult : struct
    {
        TResult Invoke(T arg0, U arg1);
    }

    public interface IValueFunc<in T, in U, in V, out TResult>
        where T : struct
        where U : struct
        where V : struct
        where TResult : struct
    {
        TResult Invoke(T arg0, U arg1, V arg2);
    }

    public interface IValueFunc<in T, in U, in V, in W, out TResult>
        where T : struct
        where U : struct
        where V : struct
        where W : struct
        where TResult : struct
    {
        TResult Invoke(T arg0, U arg1, V arg2, W arg3);
    }

    public delegate TResult PureFunc<out TResult>()
        where TResult : struct;

    public delegate TResult PureFunc<in T, out TResult>(T arg0)
        where T : struct
        where TResult : struct;

    public delegate TResult PureFunc<in T, in U, out TResult>(T arg0, U arg1)
        where T : struct
        where U : struct
        where TResult : struct;

    public delegate TResult PureFunc<in T, in U, in V, out TResult>(T arg0, U arg1, V arg2)
        where T : struct
        where U : struct
        where V : struct
        where TResult : struct;

    public delegate TResult PureFunc<in T, in U, in V, in W, out TResult>(T arg0, U arg1, V arg2, W arg3)
        where T : struct
        where U : struct
        where V : struct
        where W : struct
        where TResult : struct;
}
