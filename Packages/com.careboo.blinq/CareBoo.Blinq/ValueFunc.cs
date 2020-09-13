namespace CareBoo.Blinq
{
    public interface IFunc<out TResult>
        where TResult : struct
    {
        TResult Invoke();
    }

    public interface IFunc<in T, out TResult>
        where T : struct
        where TResult : struct
    {
        TResult Invoke(T arg0);
    }

    public interface IFunc<in T, in U, out TResult>
        where T : struct
        where U : struct
        where TResult : struct
    {
        TResult Invoke(T arg0, U arg1);
    }

    public interface IFunc<in T, in U, in V, out TResult>
        where T : struct
        where U : struct
        where V : struct
        where TResult : struct
    {
        TResult Invoke(T arg0, U arg1, V arg2);
    }

    public interface IFunc<in T, in U, in V, in W, out TResult>
        where T : struct
        where U : struct
        where V : struct
        where W : struct
        where TResult : struct
    {
        TResult Invoke(T arg0, U arg1, V arg2, W arg3);
    }

    public struct ValueFunc<TResult>
        where TResult : struct
    {
        public struct Reference<TFunc>
            where TFunc : struct, IFunc<TResult>
        {
            readonly TFunc func;

            public TResult Invoke()
            {
                return func.Invoke();
            }

            internal Reference(TFunc func)
            {
                this.func = func;
            }
        }

        public static Reference<TFunc> CreateReference<TFunc>(TFunc func = default)
            where TFunc : struct, IFunc<TResult>
        {
            return new Reference<TFunc>(func);
        }
    }

    public struct ValueFunc<T, TResult>
        where T : struct
        where TResult : struct
    {
        public struct Reference<TFunc>
            where TFunc : struct, IFunc<T, TResult>
        {
            readonly TFunc func;

            public TResult Invoke(T arg0)
            {
                return func.Invoke(arg0);
            }

            internal Reference(TFunc func)
            {
                this.func = func;
            }
        }

        public static Reference<TFunc> CreateReference<TFunc>(TFunc func = default)
            where TFunc : struct, IFunc<T, TResult>
        {
            return new Reference<TFunc>(func);
        }
    }

    public struct ValueFunc<T, U, TResult>
        where T : struct
        where U : struct
        where TResult : struct
    {
        public struct Reference<TFunc>
            where TFunc : struct, IFunc<T, U, TResult>
        {
            readonly TFunc func;

            public TResult Invoke(T arg0, U arg1)
            {
                return func.Invoke(arg0, arg1);
            }

            internal Reference(TFunc func)
            {
                this.func = func;
            }
        }

        public static Reference<TFunc> CreateReference<TFunc>(TFunc func = default)
            where TFunc : struct, IFunc<T, U, TResult>
        {
            return new Reference<TFunc>(func);
        }
    }

    public struct ValueFunc<T, U, V, TResult>
        where T : struct
        where U : struct
        where V : struct
        where TResult : struct
    {
        public struct Reference<TFunc>
            where TFunc : struct, IFunc<T, U, V, TResult>
        {
            readonly TFunc func;

            public TResult Invoke(T arg0, U arg1, V arg2)
            {
                return func.Invoke(arg0, arg1, arg2);
            }

            internal Reference(TFunc func)
            {
                this.func = func;
            }
        }

        public static Reference<TFunc> CreateReference<TFunc>(TFunc func = default)
            where TFunc : struct, IFunc<T, U, V, TResult>
        {
            return new Reference<TFunc>(func);
        }
    }
}
