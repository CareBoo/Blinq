using System.Collections.Generic;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public partial struct ValueSequence<T, TQuery>
        : IEnumerable<T>
        where T : struct
        where TQuery : struct, IQuery<T>
    {
        public struct SelectWithIndexQuery<TResult, TSelector> : IQuery<TResult>
            where TResult : struct
            where TSelector : struct, IValueFunc<T, int, TResult>
        {
            public TQuery Query;
            public TSelector Selector;

            public NativeList<TResult> Execute()
            {
                var sourceList = Query.Execute();
                var newList = new NativeList<TResult>(sourceList.Length, Allocator.Temp);
                for (var i = 0; i < sourceList.Length; i++)
                {
                    newList[i] = Selector.Invoke(sourceList[i], i);
                }
                sourceList.Dispose();
                return newList;
            }
        }

        public ValueSequence<TResult, SelectWithIndexQuery<TResult, TSelector>> SelectWithIndex<TResult, TSelector>(TSelector selector = default)
            where TResult : struct
            where TSelector : struct, IValueFunc<T, int, TResult>
        {
            var newQuery = new SelectWithIndexQuery<TResult, TSelector> { Query = query, Selector = selector };
            return new ValueSequence<TResult, SelectWithIndexQuery<TResult, TSelector>>(newQuery);
        }

        public struct SelectQuery<TResult, TSelector> : IQuery<TResult>
            where TResult : struct
            where TSelector : struct, IValueFunc<T, TResult>
        {
            public TQuery Query;
            public TSelector Selector;

            public NativeList<TResult> Execute()
            {
                var sourceList = Query.Execute();
                var newList = new NativeList<TResult>(sourceList.Length, Allocator.Temp);
                for (var i = 0; i < sourceList.Length; i++)
                {
                    newList[i] = Selector.Invoke(sourceList[i]);
                }
                sourceList.Dispose();
                return newList;
            }
        }

        public ValueSequence<TResult, SelectQuery<TResult, TSelector>> Select<TResult, TSelector>(TSelector selector = default)
            where TResult : struct
            where TSelector : struct, IValueFunc<T, TResult>
        {
            var newQuery = new SelectQuery<TResult, TSelector> { Query = query, Selector = selector };
            return new ValueSequence<TResult, SelectQuery<TResult, TSelector>>(newQuery);
        }
    }
}
