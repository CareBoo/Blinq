namespace CareBoo.Blinq
{
    public static class UtilFunctions
    {
        public static ValueFunc<T, TResult, TResult>.Impl<RightSelector<T, TResult>> RightSelector<T, TResult>()
            where T : struct
            where TResult : struct
        {
            return ValueFunc<T, TResult, TResult>.CreateImpl<RightSelector<T, TResult>>();
        }

        public static ValueFunc<T, TResult, T>.Impl<LeftSelector<T, TResult>> LeftSelector<T, TResult>()
            where T : struct
            where TResult : struct
        {
            return ValueFunc<T, TResult, T>.CreateImpl<LeftSelector<T, TResult>>();
        }

        public static ValueFunc<T, T>.Impl<SameSelector<T>> SameSelector<T>()
            where T : struct
        {
            return ValueFunc<T, T>.CreateImpl<SameSelector<T>>();
        }
    }
}
