using System;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public partial struct ValueSequence<T, TSource>
    {
        public struct SelectWithIndexSequence<TResult> : ISequence<TResult>
            where TResult : unmanaged, IEquatable<TResult>
        {
            public TSource Source;
            public ValueFunc<T, int, TResult> Selector;

            public NativeList<TResult> Execute()
            {
                var sourceList = Source.Execute();

                var newList = new NativeList<TResult>(sourceList.Length, Allocator.Temp);
                for (var i = 0; i < sourceList.Length; i++)
                {
                    newList.AddNoResize(Selector.Invoke(sourceList[i], i));
                }
                sourceList.Dispose();
                return newList;
            }
        }

        public ValueSequence<TResult, SelectWithIndexSequence<TResult>> Select<TResult>(ValueFunc<T, int, TResult> selector)
            where TResult : unmanaged, IEquatable<TResult>
        {
            var newSequence = new SelectWithIndexSequence<TResult> { Source = source, Selector = selector };
            return Create<TResult, SelectWithIndexSequence<TResult>>(newSequence);
        }

        public struct SelectSequence<TResult> : ISequence<TResult>
            where TResult : unmanaged, IEquatable<TResult>
        {
            public TSource Source;
            public ValueFunc<T, TResult> Selector;

            public NativeList<TResult> Execute()
            {
                var sourceList = Source.Execute();
                var newList = new NativeList<TResult>(sourceList.Length, Allocator.Temp);
                for (var i = 0; i < sourceList.Length; i++)
                {
                    newList.AddNoResize(Selector.Invoke(sourceList[i]));
                }
                sourceList.Dispose();
                return newList;
            }
        }

        public ValueSequence<TResult, SelectSequence<TResult>> Select<TResult>(ValueFunc<T, TResult> selector)
            where TResult : unmanaged, IEquatable<TResult>
        {
            var newSequence = new SelectSequence<TResult> { Source = source, Selector = selector };
            return Create<TResult, SelectSequence<TResult>>(newSequence);
        }
    }
}
