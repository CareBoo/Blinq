using System;
using Unity.Collections;
using Unity.Mathematics;

namespace CareBoo.Blinq
{
    public partial struct ValueSequence<T, TSource>
        where T : struct, IEquatable<T>
        where TSource : struct, ISequence<T>
    {
        public struct ZipSequence<TSecondElement, TResult, TResultSelector, TSecond> : ISequence<TResult>
            where TSecondElement : struct
            where TResult : struct, IEquatable<TResult>
            where TResultSelector : struct, IValueFunc<T, TSecondElement, TResult>
            where TSecond : struct, ISequence<TSecondElement>
        {
            public TSource Source;
            public TSecond Second;
            public TResultSelector ResultSelector;

            public NativeList<TResult> Execute()
            {
                var first = Source.Execute();
                var second = Second.Execute();
                var length = math.min(first.Length, second.Length);
                var result = new NativeList<TResult>(length, Allocator.Temp);
                for (var i = 0; i < length; i++)
                {
                    result.AddNoResize(ResultSelector.Invoke(first[i], second[i]));
                }
                first.Dispose();
                second.Dispose();
                return result;
            }
        }

        public ValueSequence<TResult, ZipSequence<TSecondElement, TResult, TResultSelector, TSecond>> Zip<TSecondElement, TResult, TResultSelector, TSecond>(TSecond second, TResultSelector resultSelector)
            where TSecondElement : struct
            where TResult : struct, IEquatable<TResult>
            where TResultSelector : struct, IValueFunc<T, TSecondElement, TResult>
            where TSecond : struct, ISequence<TSecondElement>
        {
            var newSequence = new ZipSequence<TSecondElement, TResult, TResultSelector, TSecond> { Source = source, Second = second, ResultSelector = resultSelector };
            return new ValueSequence<TResult, ZipSequence<TSecondElement, TResult, TResultSelector, TSecond>>(newSequence);
        }
    }
}
