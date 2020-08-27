using Unity.Burst;

namespace CareBoo.Blinq
{
    public struct BurstCompiledFunc<TResult> : IFunc<TResult>
        where TResult : struct
    {
        public delegate TResult Delegate();

        private readonly FunctionPointer<Delegate> functionPointer;

        public BurstCompiledFunc(FunctionPointer<Delegate> functionPointer)
        {
            this.functionPointer = functionPointer;
        }

        public TResult Invoke()
        {
            return functionPointer.Invoke();
        }

        public static BurstCompiledFunc<TResult> Compile(Delegate func)
        {
            var functionPointer = BurstCompiler.CompileFunctionPointer(func);
            return new BurstCompiledFunc<TResult>(functionPointer);
        }
    }

    public struct BurstCompiledFunc<T, TResult> : IFunc<T, TResult>, IFunc<TResult>
        where T : struct
        where TResult : struct
    {
        public delegate TResult Delegate(T arg0);

        public T Arg0;

        private readonly FunctionPointer<Delegate> functionPointer;

        public BurstCompiledFunc(FunctionPointer<Delegate> functionPointer)
        {
            Arg0 = default;
            this.functionPointer = functionPointer;
        }

        public TResult Invoke(T arg0)
        {
            return functionPointer.Invoke(arg0);
        }

        public TResult Invoke()
        {
            return functionPointer.Invoke(Arg0);
        }

        public static BurstCompiledFunc<T, TResult> Compile(Delegate func)
        {
            var functionPointer = BurstCompiler.CompileFunctionPointer(func);
            return new BurstCompiledFunc<T, TResult>(functionPointer);
        }
    }

    public struct BurstCompiledFunc<T, U, TResult> : IFunc<T, U, TResult>, IFunc<T, TResult>, IFunc<TResult>
        where T : struct
        where U : struct
        where TResult : struct
    {
        public delegate TResult Delegate(T arg0, U arg1);

        public T Arg0;
        public U Arg1;

        private readonly FunctionPointer<Delegate> functionPointer;

        public BurstCompiledFunc(FunctionPointer<Delegate> functionPointer)
        {
            Arg0 = default;
            Arg1 = default;
            this.functionPointer = functionPointer;
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

        public static BurstCompiledFunc<T, U, TResult> Compile(Delegate func)
        {
            var functionPointer = BurstCompiler.CompileFunctionPointer(func);
            return new BurstCompiledFunc<T, U, TResult>(functionPointer);
        }
    }

    public struct BurstCompiledFunc<T, U, V, TResult> : IFunc<T, U, V, TResult>, IFunc<T, U, TResult>, IFunc<T, TResult>, IFunc<TResult>
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

        public BurstCompiledFunc(FunctionPointer<Delegate> functionPointer)
        {
            Arg0 = default;
            Arg1 = default;
            Arg2 = default;
            this.functionPointer = functionPointer;
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

        public static BurstCompiledFunc<T, U, V, TResult> Compile(Delegate func)
        {
            var functionPointer = BurstCompiler.CompileFunctionPointer(func);
            return new BurstCompiledFunc<T, U, V, TResult>(functionPointer);
        }
    }
}
