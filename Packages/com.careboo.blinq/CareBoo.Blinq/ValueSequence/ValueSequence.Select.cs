using System;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public partial struct ValueSequence<T, TSource>
    {
        public struct SelectWithIndexSequence<TResult, TPredicate> : ISequence<TResult>
            where TResult : unmanaged, IEquatable<TResult>
            where TPredicate : struct, IFunc<T, int, TResult>
        {
            public TSource Source;
            public ValueFunc<T, int, TResult>.Impl<TPredicate> Selector;

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

        public ValueSequence<TResult, SelectWithIndexSequence<TResult, TPredicate>> Select<TResult, TPredicate>(ValueFunc<T, int, TResult>.Impl<TPredicate> selector)
            where TResult : unmanaged, IEquatable<TResult>
            where TPredicate : struct, IFunc<T, int, TResult>
        {
            var newSequence = new SelectWithIndexSequence<TResult, TPredicate> { Source = source, Selector = selector };
            return Create<TResult, SelectWithIndexSequence<TResult, TPredicate>>(newSequence);
        }

        public struct SelectSequence<TResult, TPredicate> : ISequence<TResult>
            where TResult : unmanaged, IEquatable<TResult>
            where TPredicate : struct, IFunc<T, TResult>
        {
            public TSource Source;
            public ValueFunc<T, TResult>.Impl<TPredicate> Selector;

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

        public ValueSequence<TResult, SelectSequence<TResult, TPredicate>> Select<TResult, TPredicate>(ValueFunc<T, TResult>.Impl<TPredicate> selector)
            where TResult : unmanaged, IEquatable<TResult>
            where TPredicate : struct, IFunc<T, TResult>
        {
            var newSequence = new SelectSequence<TResult, TPredicate> { Source = source, Selector = selector };
            return Create<TResult, SelectSequence<TResult, TPredicate>>(newSequence);
        }
    }
}
