using System;
using System.Collections.Generic;
using CareBoo.Burst.Delegates;
using Unity.Collections;
using Unity.Jobs;

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

        public struct SequenceMaxFunc<T, TSource, TSourceEnumerator, TResult, TSelector>
            : IFunc<ValueSequence<T, TSource, TSourceEnumerator>, TResult>
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TResult : struct, IComparable<TResult>
            where TSelector : struct, IFunc<T, TResult>
        {
            public ValueFunc<T, TResult>.Struct<TSelector> Selector;

            public TResult Invoke(ValueSequence<T, TSource, TSourceEnumerator> arg0)
            {
                return arg0.Max(Selector);
            }
        }

        public static ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, TResult>.Struct<SequenceMaxFunc<T, TSource, TSourceEnumerator, TResult, TSelector>>
        MaxAsFunc<T, TSource, TSourceEnumerator, TResult, TSelector>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, TResult>.Struct<TSelector> selector
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TResult : struct, IComparable<TResult>
            where TSelector : struct, IFunc<T, TResult>
        {
            var func = new SequenceMaxFunc<T, TSource, TSourceEnumerator, TResult, TSelector> { Selector = selector };
            return ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, TResult>.New(func);
        }

        public static TResult RunMax<T, TSource, TSourceEnumerator, TResult, TSelector>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, TResult>.Struct<TSelector> selector
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TResult : struct, IComparable<TResult>
            where TSelector : struct, IFunc<T, TResult>
        {
            var func = source.MaxAsFunc(selector);
            return source.Run(func);
        }

        public static JobHandle<TResult> ScheduleMax<T, TSource, TSourceEnumerator, TResult, TSelector>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, TResult>.Struct<TSelector> selector
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TResult : struct, IComparable<TResult>
            where TSelector : struct, IFunc<T, TResult>
        {
            var func = source.MaxAsFunc(selector);
            return source.Schedule(func);
        }

        public static JobHandle ScheduleMax<T, TSource, TSourceEnumerator, TResult, TSelector>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ref NativeArray<TResult> output,
            ValueFunc<T, TResult>.Struct<TSelector> selector
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TResult : struct, IComparable<TResult>
            where TSelector : struct, IFunc<T, TResult>
        {
            var func = source.MaxAsFunc(selector);
            return source.Schedule(func, ref output);
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

        public struct SequenceMaxFunc<T, TSource, TSourceEnumerator>
            : IFunc<ValueSequence<T, TSource, TSourceEnumerator>, T>
            where T : struct, IComparable<T>
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            public T Invoke(ValueSequence<T, TSource, TSourceEnumerator> arg0)
            {
                return arg0.Max();
            }
        }

        public static ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, T>.Struct<SequenceMaxFunc<T, TSource, TSourceEnumerator>>
        MaxAsFunc<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source
            )
            where T : struct, IComparable<T>
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            return ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, T>.New<SequenceMaxFunc<T, TSource, TSourceEnumerator>>();
        }

        public static T RunMax<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source
            )
            where T : struct, IComparable<T>
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var func = source.MaxAsFunc();
            return source.Run(func);
        }

        public static JobHandle<T> ScheduleMax<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source
            )
            where T : struct, IComparable<T>
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var func = source.MaxAsFunc();
            return source.Schedule(func);
        }

        public static JobHandle ScheduleMax<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ref NativeArray<T> output
            )
            where T : struct, IComparable<T>
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var func = source.MaxAsFunc();
            return source.Schedule(func, ref output);
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

        public struct SequenceMinFunc<T, TSource, TSourceEnumerator, TResult, TSelector>
            : IFunc<ValueSequence<T, TSource, TSourceEnumerator>, TResult>
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TResult : struct, IComparable<TResult>
            where TSelector : struct, IFunc<T, TResult>
        {
            public ValueFunc<T, TResult>.Struct<TSelector> Selector;

            public TResult Invoke(ValueSequence<T, TSource, TSourceEnumerator> arg0)
            {
                return arg0.Min(Selector);
            }
        }

        public static ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, TResult>.Struct<SequenceMinFunc<T, TSource, TSourceEnumerator, TResult, TSelector>>
        MinAsFunc<T, TSource, TSourceEnumerator, TResult, TSelector>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, TResult>.Struct<TSelector> selector
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TResult : struct, IComparable<TResult>
            where TSelector : struct, IFunc<T, TResult>
        {
            var func = new SequenceMinFunc<T, TSource, TSourceEnumerator, TResult, TSelector> { Selector = selector };
            return ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, TResult>.New(func);
        }

        public static TResult RunMin<T, TSource, TSourceEnumerator, TResult, TSelector>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, TResult>.Struct<TSelector> selector
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TResult : struct, IComparable<TResult>
            where TSelector : struct, IFunc<T, TResult>
        {
            var func = source.MinAsFunc(selector);
            return source.Run(func);
        }

        public static JobHandle<TResult> ScheduleMin<T, TSource, TSourceEnumerator, TResult, TSelector>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, TResult>.Struct<TSelector> selector
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TResult : struct, IComparable<TResult>
            where TSelector : struct, IFunc<T, TResult>
        {
            var func = source.MinAsFunc(selector);
            return source.Schedule(func);
        }

        public static JobHandle ScheduleMin<T, TSource, TSourceEnumerator, TResult, TSelector>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ref NativeArray<TResult> output,
            ValueFunc<T, TResult>.Struct<TSelector> selector
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TResult : struct, IComparable<TResult>
            where TSelector : struct, IFunc<T, TResult>
        {
            var func = source.MinAsFunc(selector);
            return source.Schedule(func, ref output);
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

        public struct SequenceMinFunc<T, TSource, TSourceEnumerator>
            : IFunc<ValueSequence<T, TSource, TSourceEnumerator>, T>
            where T : struct, IComparable<T>
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            public T Invoke(ValueSequence<T, TSource, TSourceEnumerator> arg0)
            {
                return arg0.Max();
            }
        }

        public static ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, T>.Struct<SequenceMinFunc<T, TSource, TSourceEnumerator>>
        MinAsFunc<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source
            )
            where T : struct, IComparable<T>
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            return ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, T>.New<SequenceMinFunc<T, TSource, TSourceEnumerator>>();
        }

        public static T RunMin<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source
            )
            where T : struct, IComparable<T>
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var func = source.MinAsFunc();
            return source.Run(func);
        }

        public static JobHandle<T> ScheduleMin<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source
            )
            where T : struct, IComparable<T>
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var func = source.MinAsFunc();
            return source.Schedule(func);
        }

        public static JobHandle ScheduleMin<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ref NativeArray<T> output
            )
            where T : struct, IComparable<T>
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var func = source.MinAsFunc();
            return source.Schedule(func, ref output);
        }
    }
}
