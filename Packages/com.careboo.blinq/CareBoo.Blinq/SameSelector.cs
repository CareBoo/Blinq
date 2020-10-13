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
}
