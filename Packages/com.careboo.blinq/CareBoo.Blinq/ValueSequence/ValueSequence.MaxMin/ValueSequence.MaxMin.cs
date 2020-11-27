using System;
using System.Collections.Generic;
using CareBoo.Burst.Delegates;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static TResult Max<T, TSource, TSourceEnumerator, TResult, TSelector>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, TResult>.Struct<TSelector> selector
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TResult : struct, IComparable<TResult>
            where TSelector : struct, IFunc<T, TResult>
        {
            var srcList = source.ToNativeList(Allocator.Temp);
            if (srcList.Length == 0)
            {
                srcList.Dispose();
                throw Error.NoElements();
            }
            var max = selector.Invoke(srcList[0]);
            for (var i = 1; i < srcList.Length; i++)
            {
                var val = selector.Invoke(srcList[i]);
                if (val.CompareTo(max) > 0)
                    max = val;
            }
            srcList.Dispose();
            return max;
        }

        public static T Max<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source
            )
            where T : struct, IComparable<T>
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var srcList = source.ToNativeList(Allocator.Temp);
            if (srcList.Length == 0)
            {
                srcList.Dispose();
                throw Error.NoElements();
            }
            var max = srcList[0];
            for (var i = 1; i < srcList.Length; i++)
            {
                var val = srcList[i];
                if (val.CompareTo(max) > 0)
                    max = val;
            }
            srcList.Dispose();
            return max;
        }

        public static TResult Min<T, TSource, TSourceEnumerator, TResult, TSelector>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, TResult>.Struct<TSelector> selector
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TResult : struct, IComparable<TResult>
            where TSelector : struct, IFunc<T, TResult>
        {
            var srcList = source.ToNativeList(Allocator.Temp);
            if (srcList.Length == 0)
            {
                srcList.Dispose();
                throw Error.NoElements();
            }
            var min = selector.Invoke(srcList[0]);
            for (var i = 1; i < srcList.Length; i++)
            {
                var val = selector.Invoke(srcList[i]);
                if (val.CompareTo(min) < 0)
                    min = val;
            }
            srcList.Dispose();
            return min;
        }

        public static T Min<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source
            )
            where T : struct, IComparable<T>
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var srcList = source.ToNativeList(Allocator.Temp);
            if (srcList.Length == 0)
            {
                srcList.Dispose();
                throw Error.NoElements();
            }
            var min = srcList[0];
            for (var i = 1; i < srcList.Length; i++)
            {
                var val = srcList[i];
                if (val.CompareTo(min) < 0)
                    min = val;
            }
            srcList.Dispose();
            return min;
        }
    }
}
