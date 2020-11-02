﻿using Unity.Collections;
using CareBoo.Burst.Delegates;
using System.Collections;
using System;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<TResult, SelectIndexSequence<T, TSource, TResult, TSelector>> Select<T, TSource, TResult, TSelector>(
            this ref ValueSequence<T, TSource> source,
            in ValueFunc<T, int, TResult>.Struct<TSelector> selector
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TResult : struct
            where TSelector : struct, IFunc<T, int, TResult>
        {
            var sourceSeq = source.GetEnumerator();
            var seq = new SelectIndexSequence<T, TSource, TResult, TSelector>(ref sourceSeq, in selector);
            return ValueSequence<TResult>.New(ref seq);
        }

        public static ValueSequence<TResult, SelectSequence<T, TSource, TResult, TSelector>> Select<T, TSource, TResult, TSelector>(
            this ref ValueSequence<T, TSource> source,
            in ValueFunc<T, TResult>.Struct<TSelector> selector
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TResult : struct
            where TSelector : struct, IFunc<T, TResult>
        {
            var sourceSeq = source.GetEnumerator();
            var seq = new SelectSequence<T, TSource, TResult, TSelector>(ref sourceSeq, in selector);
            return ValueSequence<TResult>.New(ref seq);
        }
    }

    public struct SelectIndexSequence<T, TSource, TResult, TSelector> : ISequence<TResult>
        where T : struct
        where TSource : struct, ISequence<T>
        where TResult : struct
        where TSelector : struct, IFunc<T, int, TResult>
    {
        TSource source;
        readonly ValueFunc<T, int, TResult>.Struct<TSelector> selector;

        int currentIndex;

        public SelectIndexSequence(ref TSource source, in ValueFunc<T, int, TResult>.Struct<TSelector> selector)
        {
            this.source = source;
            this.selector = selector;
            currentIndex = -1;
        }

        public TResult Current => selector.Invoke(source.Current, currentIndex);

        object IEnumerator.Current => Current;

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            currentIndex += 1;
            return source.MoveNext();
        }

        public void Reset()
        {
            throw new NotSupportedException();
        }

        public NativeList<TResult> ToList()
        {
            var sourceList = source.ToList();

            var newList = new NativeList<TResult>(sourceList.Length, Allocator.Temp);
            for (var i = 0; i < sourceList.Length; i++)
                newList.AddNoResize(selector.Invoke(sourceList[i], i));
            sourceList.Dispose();
            return newList;
        }
    }

    public struct SelectSequence<T, TSource, TResult, TSelector> : ISequence<TResult>
        where T : struct
        where TSource : struct, ISequence<T>
        where TResult : struct
        where TSelector : struct, IFunc<T, TResult>
    {
        TSource source;
        readonly ValueFunc<T, TResult>.Struct<TSelector> selector;

        public SelectSequence(ref TSource source, in ValueFunc<T, TResult>.Struct<TSelector> selector)
        {
            this.source = source;
            this.selector = selector;
        }

        public TResult Current => selector.Invoke(source.Current);

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            source.Dispose();
        }

        public bool MoveNext()
        {
            return source.MoveNext();
        }

        public void Reset()
        {
            throw new NotSupportedException();
        }

        public NativeList<TResult> ToList()
        {
            var sourceList = source.ToList();

            var newList = new NativeList<TResult>(sourceList.Length, Allocator.Temp);
            for (var i = 0; i < sourceList.Length; i++)
                newList.AddNoResize(selector.Invoke(sourceList[i]));
            sourceList.Dispose();
            return newList;
        }
    }
}
