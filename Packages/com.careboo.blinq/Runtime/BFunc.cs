using System;
using Unity.Burst;

namespace CareBoo.Blinq
{
    public struct BFunc<TResult> : IFunc<TResult>
        where TResult : struct
    {
        public delegate TResult Delegate();

        private readonly FunctionPointer<Delegate> functionPointer;

        public BFunc(Delegate func)
        {
            functionPointer = BurstCompiler.CompileFunctionPointer(func);
        }

        public TResult Invoke()
        {
            return functionPointer.Invoke();
        }
    }

    public struct BFunc<T, TResult> : IFunc<T, TResult>, IFunc<TResult>
        where T : struct
        where TResult : struct
    {
        public delegate TResult Delegate(T arg0);

        public T Arg0;

        private readonly FunctionPointer<Delegate> functionPointer;

        public BFunc(Delegate func)
        {
            Arg0 = default;
            functionPointer = BurstCompiler.CompileFunctionPointer(func);
        }

        public TResult Invoke()
        {
            return functionPointer.Invoke(Arg0);
        }

        public TResult Invoke(T arg0)
        {
            return functionPointer.Invoke(arg0);
        }
    }

    public struct BFunc<T, U, TResult> : IFunc<T, U, TResult>, IFunc<T, TResult>, IFunc<TResult>
        where T : struct
        where U : struct
        where TResult : struct
    {
        public delegate TResult Delegate(T arg0, U arg1);

        public T Arg0;
        public U Arg1;

        private readonly FunctionPointer<Delegate> functionPointer;

        public BFunc(Delegate func)
        {
            Arg0 = default;
            Arg1 = default;
            functionPointer = BurstCompiler.CompileFunctionPointer(func);
        }

        public TResult Invoke(T arg0, U arg1)
        {
            return functionPointer.Invoke(arg0, arg1);
        }

        public TResult Invoke(T arg0)
        {
            return functionPointer.Invoke(arg0, Arg1);
        }

        public TResult Invoke()
        {
            return functionPointer.Invoke(Arg0, Arg1);
        }
    }

    public struct BFunc<T, U, V, TResult> : IFunc<T, U, V, TResult>, IFunc<T, U, TResult>, IFunc<T, TResult>, IFunc<TResult>
        where T : struct
        where U : struct
        where V : struct
        where TResult : struct
    {
        public delegate TResult Delegate(T arg0, U arg1, V arg2);

        public T Arg0;
        public U Arg1;
        public V Arg2;

        private readonly FunctionPointer<Delegate> functionPointer;

        public BFunc(Delegate func)
        {
            Arg0 = default;
            Arg1 = default;
            Arg2 = default;
            functionPointer = BurstCompiler.CompileFunctionPointer(func);
        }

        public TResult Invoke(T arg0, U arg1, V arg2)
        {
            return functionPointer.Invoke(arg0, arg1, arg2);
        }

        public TResult Invoke(T arg0, U arg1)
        {
            return functionPointer.Invoke(arg0, arg1, Arg2);
        }

        public TResult Invoke(T arg0)
        {
            return functionPointer.Invoke(arg0, Arg1, Arg2);
        }

        public TResult Invoke()
        {
            return functionPointer.Invoke(Arg0, Arg1, Arg2);
        }
    }
}
