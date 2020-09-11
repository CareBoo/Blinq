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

    public struct ValueFunc<TResult>
        where TResult : struct
    {
        public delegate TResult Func();

        readonly FunctionPointer<Func> functionPtr;

        public ValueFunc(FunctionPointer<Func> functionPtr)
        {
            this.functionPtr = functionPtr;
        }

        public TResult Invoke()
        {
            return functionPtr.Invoke();
        }

        public static ValueFunc<TResult> Compile(Func func)
        {
            var functionPtr = BurstCompiler.CompileFunctionPointer(func);
            return new ValueFunc<TResult>(functionPtr);
        }
    }

    public struct ValueFunc<T, TResult>
        where T : struct
        where TResult : struct
    {
        public delegate TResult Func(T arg0);

        readonly FunctionPointer<Func> functionPtr;

        public ValueFunc(FunctionPointer<Func> functionPtr)
        {
            this.functionPtr = functionPtr;
        }

        public TResult Invoke(T arg0)
        {
            return functionPtr.Invoke(arg0);
        }

        public static ValueFunc<T, TResult> Compile(Func func)
        {
            var functionPtr = BurstCompiler.CompileFunctionPointer(func);
            return new ValueFunc<T, TResult>(functionPtr);
        }
    }

    public struct ValueFunc<T, U, TResult>
        where T : struct
        where U : struct
        where TResult : struct
    {
        public delegate TResult Func(T arg0, U arg1);

        readonly FunctionPointer<Func> functionPtr;

        public ValueFunc(FunctionPointer<Func> functionPtr)
        {
            this.functionPtr = functionPtr;
        }

        public TResult Invoke(T arg0, U arg1)
        {
            return functionPtr.Invoke(arg0, arg1);
        }

        public static ValueFunc<T, U, TResult> Compile(Func func)
        {
            var functionPtr = BurstCompiler.CompileFunctionPointer(func);
            return new ValueFunc<T, U, TResult>(functionPtr);
        }
    }

    public struct ValueFunc<T, U, V, TResult>
        where T : struct
        where U : struct
        where V : struct
        where TResult : struct
    {
        public delegate TResult Func(T arg0, U arg1, V arg2);

        readonly FunctionPointer<Func> functionPtr;

        public ValueFunc(FunctionPointer<Func> functionPtr)
        {
            this.functionPtr = functionPtr;
        }

        public TResult Invoke(T arg0, U arg1, V arg2)
        {
            return functionPtr.Invoke(arg0, arg1, arg2);
        }

        public static ValueFunc<T, U, V, TResult> Compile(Func func)
        {
            var functionPtr = BurstCompiler.CompileFunctionPointer(func);
            return new ValueFunc<T, U, V, TResult>(functionPtr);
        }
    }

    public static class ValueFuncCompiler
    {
        public static ValueFunc<TResult> Compile<TResult>(ValueFunc<TResult>.Func func)
            where TResult : struct
        {
            var functionPtr = BurstCompiler.CompileFunctionPointer(func);
            return new ValueFunc<TResult>(functionPtr);
        }

        public static ValueFunc<T, TResult> Compile<T, TResult>(ValueFunc<T, TResult>.Func func)
            where T : struct
            where TResult : struct
        {
            var functionPtr = BurstCompiler.CompileFunctionPointer(func);
            return new ValueFunc<T, TResult>(functionPtr);
        }

        public static ValueFunc<T, U, TResult> Compile<T, U, TResult>(ValueFunc<T, U, TResult>.Func func)
            where T : struct
            where U : struct
            where TResult : struct
        {
            var functionPtr = BurstCompiler.CompileFunctionPointer(func);
            return new ValueFunc<T, U, TResult>(functionPtr);
        }

        public static ValueFunc<T, U, V, TResult> Compile<T, U, V, TResult>(ValueFunc<T, U, V, TResult>.Func func)
            where T : struct
            where U : struct
            where V : struct
            where TResult : struct
        {
            var functionPtr = BurstCompiler.CompileFunctionPointer(func);
            return new ValueFunc<T, U, V, TResult>(functionPtr);
        }
    }
}
