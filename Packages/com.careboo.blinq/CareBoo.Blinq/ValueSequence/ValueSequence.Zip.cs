using System;
using Unity.Collections;
using Unity.Mathematics;

namespace CareBoo.Blinq
{
    public partial struct ValueSequence<T, TSource>
    {
        public struct ZipSequence<TSecondElement, TResult, TSecond> : ISequence<TResult>
            where TSecondElement : struct
            where TResult : struct, IEquatable<TResult>
            where TSecond : struct, ISequence<TSecondElement>
        {
            public TSource Source;
            public TSecond Second;
            public ValueFunc<T, TSecondElement, TResult> ResultSelector;

            public NativeList<TResult> Execute()
            {
                var source = Source.Execute();
                var second = Second.Execute();
                var length = math.min(source.Length, second.Length);
                var result = new NativeList<TResult>(length, Allocator.Temp);
                for (var i = 0; i < length; i++)
                {
                    result.AddNoResize(ResultSelector.Invoke(source[i], second[i]));
                }
                source.Dispose();
                second.Dispose();
                return result;
            }
        }

        public ValueSequence<TResult, ZipSequence<TSecondElement, TResult, TSecond>> Zip<TSecondElement, TResult, TSecond>(TSecond second, ValueFunc<T, TSecondElement, TResult> resultSelector)
            where TSecondElement : struct
            where TResult : unmanaged, IEquatable<TResult>
            where TSecond : struct, ISequence<TSecondElement>
        {
            var newSequence = new ZipSequence<TSecondElement, TResult, TSecond> { Source = source, Second = second, ResultSelector = resultSelector };
            return Create<TResult, ZipSequence<TSecondElement, TResult, TSecond>>(newSequence);
        }
    }
}
