using System;
using CareBoo.Burst.Delegates;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static TResult Max<T, TSource, TResult, TSelector>(
            this ValueSequence<T, TSource> source,
            ValueFunc<T, TResult>.Struct<TSelector> selector
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TResult : struct, IComparable<TResult>
            where TSelector : struct, IFunc<T, TResult>
        {
            using (var srcList = source.Execute())
            {
                if (srcList.Length == 0) throw Error.NoElements();
                var max = selector.Invoke(srcList[0]);
                for (var i = 1; i < srcList.Length; i++)
                {
                    var val = selector.Invoke(srcList[i]);
                    if (val.CompareTo(max) > 0)
                        max = val;
                }
                return max;
            }
        }

        public static T Max<T, TSource>(
            this ValueSequence<T, TSource> source
            )
            where T : struct, IComparable<T>
            where TSource : struct, ISequence<T>
        {
            using (var srcList = source.Execute())
            {
                if (srcList.Length == 0) throw Error.NoElements();
                var max = srcList[0];
                for (var i = 1; i < srcList.Length; i++)
                {
                    var val = srcList[i];
                    if (val.CompareTo(max) > 0)
                        max = val;
                }
                return max;
            }
        }

        public static TResult Min<T, TSource, TResult, TSelector>(
            this ValueSequence<T, TSource> source,
            ValueFunc<T, TResult>.Struct<TSelector> selector
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TResult : struct, IComparable<TResult>
            where TSelector : struct, IFunc<T, TResult>
        {
            using (var srcList = source.Execute())
            {
                if (srcList.Length == 0) throw Error.NoElements();
                var min = selector.Invoke(srcList[0]);
                for (var i = 1; i < srcList.Length; i++)
                {
                    var val = selector.Invoke(srcList[i]);
                    if (val.CompareTo(min) < 0)
                        min = val;
                }
                return min;
            }
        }

        public static T Min<T, TSource>(
            this ValueSequence<T, TSource> source
            )
            where T : struct, IComparable<T>
            where TSource : struct, ISequence<T>
        {
            using (var srcList = source.Execute())
            {
                if (srcList.Length == 0) throw Error.NoElements();
                var min = srcList[0];
                for (var i = 1; i < srcList.Length; i++)
                {
                    var val = srcList[i];
                    if (val.CompareTo(min) < 0)
                        min = val;
                }
                return min;
            }
        }
    }
}
