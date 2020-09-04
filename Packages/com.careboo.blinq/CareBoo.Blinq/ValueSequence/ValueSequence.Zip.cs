using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;

namespace CareBoo.Blinq
{
    public partial struct ValueSequence<T, TQuery>
        : IEnumerable<T>
        where T : struct
        where TQuery : struct, IQuery<T>
    {
        public struct ZipQuery<TSecond, TResult, TResultSelector, TSecondSequence> : IQuery<TResult>
            where TSecond : struct
            where TResult : struct
            where TResultSelector : struct, IValueFunc<T, TSecond, TResult>
            where TSecondSequence : struct, IValueSequence<TSecond>
        {
            public TQuery Query;
            public TSecondSequence SecondSequence;
            public TResultSelector ResultSelector;

            public NativeList<TResult> Execute()
            {
                var first = Query.Execute();
                var second = SecondSequence.ToNativeList();
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

        public ValueSequence<TResult, ZipQuery<TSecond, TResult, TResultSelector, TSecondSequence>> Zip<TSecond, TResult, TResultSelector, TSecondSequence>(TSecondSequence second, TResultSelector resultSelector)
            where TSecond : struct
            where TResult : struct
            where TResultSelector : struct, IValueFunc<T, TSecond, TResult>
            where TSecondSequence : struct, IValueSequence<TSecond>
        {
            var newQuery = new ZipQuery<TSecond, TResult, TResultSelector, TSecondSequence> { Query = query, SecondSequence = second, ResultSelector = resultSelector };
            return new ValueSequence<TResult, ZipQuery<TSecond, TResult, TResultSelector, TSecondSequence>>(newQuery);
        }
    }
}
