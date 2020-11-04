using System;
using Unity.Collections;
using CareBoo.Burst.Delegates;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static TResult Max<T, TResult, TSelector>(
            this in NativeArray<T> source,
            in ValueFunc<T, TResult>.Struct<TSelector> selector
            )
            where T : struct
            where TResult : struct, IComparable<TResult>
            where TSelector : struct, IFunc<T, TResult>
        {
            if (source.Length == 0) throw Error.NoElements();
            var max = selector.Invoke(source[0]);
            for (var i = 1; i < source.Length; i++)
            {
                var val = selector.Invoke(source[i]);
                if (val.CompareTo(max) > 0)
                    max = val;
            }
            return max;
        }

        public static T Max<T>(in NativeArray<T> source)
            where T : struct, IComparable<T>
        {
            if (source.Length == 0) throw Error.NoElements();
            var max = source[0];
            for (var i = 1; i < source.Length; i++)
            {
                var val = source[i];
                if (val.CompareTo(max) > 0)
                    max = val;
            }
            return max;
        }

        public static TResult Min<T, TResult, TSelector>(
            in NativeArray<T> source,
            in ValueFunc<T, TResult>.Struct<TSelector> selector
            )
            where T : struct
            where TResult : struct, IComparable<TResult>
            where TSelector : struct, IFunc<T, TResult>
        {
            if (source.Length == 0) throw Error.NoElements();
            var min = selector.Invoke(source[0]);
            for (var i = 1; i < source.Length; i++)
            {
                var val = selector.Invoke(source[i]);
                if (val.CompareTo(min) < 0)
                    min = val;
            }
            return min;
        }

        public static T Min<T>(in NativeArray<T> source)
            where T : struct, IComparable<T>
        {
            if (source.Length == 0) throw Error.NoElements();
            var min = source[0];
            for (var i = 1; i < source.Length; i++)
            {
                var val = source[i];
                if (val.CompareTo(min) < 0)
                    min = val;
            }
            return min;
        }
    }
}
