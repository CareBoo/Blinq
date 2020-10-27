using CareBoo.Burst.Delegates;

namespace CareBoo.Blinq
{
    public struct RightSelector<T, TResult>
        : IFunc<T, TResult, TResult>
        where T : struct
        where TResult : struct
    {
        public TResult Invoke(T arg0, TResult arg1)
        {
            return arg1;
        }
    }

    public struct LeftSelector<T, TResult>
        : IFunc<T, TResult, T>
        where T : struct
        where TResult : struct
    {
        public T Invoke(T arg0, TResult arg1)
        {
            return arg0;
        }
    }

    public struct SameSelector<T>
        : IFunc<T, T>
        where T : struct
    {
        public T Invoke(T arg0) => arg0;
    }

    public static class UtilFunctions
    {
        public static ValueFunc<T, TResult, TResult>.Struct<RightSelector<T, TResult>> RightSelector<T, TResult>()
            where T : struct
            where TResult : struct
        {
            return ValueFunc<T, TResult, TResult>.New<RightSelector<T, TResult>>();
        }

        public static ValueFunc<T, TResult, T>.Struct<LeftSelector<T, TResult>> LeftSelector<T, TResult>()
            where T : struct
            where TResult : struct
        {
            return ValueFunc<T, TResult, T>.New<LeftSelector<T, TResult>>();
        }

        public static ValueFunc<T, T>.Struct<SameSelector<T>> SameSelector<T>()
            where T : struct
        {
            return ValueFunc<T, T>.New<SameSelector<T>>();
        }
    }
}
