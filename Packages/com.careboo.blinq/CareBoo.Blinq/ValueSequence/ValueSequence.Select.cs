﻿using System;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public partial struct ValueSequence<T, TSource>
    {
        public struct SelectWithIndexSequence<TResult, TSelector> : ISequence<TResult>
            where TResult : unmanaged, IEquatable<TResult>
            where TSelector : struct, IValueFunc<T, int, TResult>
        {
            public TSource Source;
            public TSelector Selector;

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

        public ValueSequence<TResult, SelectWithIndexSequence<TResult, TSelector>> SelectWithIndex<TResult, TSelector>(TSelector selector = default)
            where TResult : unmanaged, IEquatable<TResult>
            where TSelector : struct, IValueFunc<T, int, TResult>
        {
            var newSequence = new SelectWithIndexSequence<TResult, TSelector> { Source = source, Selector = selector };
            return Create<TResult, SelectWithIndexSequence<TResult, TSelector>>(newSequence);
        }

        public struct SelectSequence<TResult, TSelector> : ISequence<TResult>
            where TResult : unmanaged, IEquatable<TResult>
            where TSelector : struct, IValueFunc<T, TResult>
        {
            public TSource Source;
            public TSelector Selector;

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

        public ValueSequence<TResult, SelectSequence<TResult, TSelector>> Select<TResult, TSelector>(TSelector selector = default)
            where TResult : unmanaged, IEquatable<TResult>
            where TSelector : struct, IValueFunc<T, TResult>
        {
            var newSequence = new SelectSequence<TResult, TSelector> { Source = source, Selector = selector };
            return Create<TResult, SelectSequence<TResult, TSelector>>(newSequence);
        }
    }
}
