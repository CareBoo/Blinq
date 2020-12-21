using System;
using Unity.Collections;
using CareBoo.Burst.Delegates;
using Unity.Jobs;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static TResult Max<T, TResult, TSelector>(
            this in NativeArray<T> source,
            ValueFunc<T, TResult>.Struct<TSelector> selector
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

        public struct ArrayMaxFunc<T, TResult, TSelector>
            : IFunc<NativeArray<T>, TResult>
            where T : struct
            where TResult : struct, IComparable<TResult>
            where TSelector : struct, IFunc<T, TResult>
        {
            public ValueFunc<T, TResult>.Struct<TSelector> Selector;

            public TResult Invoke(NativeArray<T> arg0)
            {
                return arg0.Max(Selector);
            }
        }

        public static ValueFunc<NativeArray<T>, TResult>.Struct<ArrayMaxFunc<T, TResult, TSelector>>
        MaxAsFunc<T, TResult, TSelector>(
            this in NativeArray<T> source,
            ValueFunc<T, TResult>.Struct<TSelector> selector
            )
            where T : struct
            where TResult : struct, IComparable<TResult>
            where TSelector : struct, IFunc<T, TResult>
        {
            var func = new ArrayMaxFunc<T, TResult, TSelector> { Selector = selector };
            return ValueFunc<NativeArray<T>, TResult>.New(func);
        }

        public static TResult RunMax<T, TResult, TSelector>(
            this in NativeArray<T> source,
            ValueFunc<T, TResult>.Struct<TSelector> selector
            )
            where T : struct
            where TResult : struct, IComparable<TResult>
            where TSelector : struct, IFunc<T, TResult>
        {
            var func = source.MaxAsFunc(selector);
            return source.Run(func);
        }

        public static JobHandle<TResult> ScheduleMax<T, TResult, TSelector>(
            this in NativeArray<T> source,
            ValueFunc<T, TResult>.Struct<TSelector> selector
            )
            where T : struct
            where TResult : struct, IComparable<TResult>
            where TSelector : struct, IFunc<T, TResult>
        {
            var func = source.MaxAsFunc(selector);
            return source.Schedule(func);
        }

        public static JobHandle ScheduleMax<T, TResult, TSelector>(
            this in NativeArray<T> source,
            ref NativeArray<TResult> output,
            ValueFunc<T, TResult>.Struct<TSelector> selector
            )
            where T : struct
            where TResult : struct, IComparable<TResult>
            where TSelector : struct, IFunc<T, TResult>
        {
            var func = source.MaxAsFunc(selector);
            return source.Schedule(func, ref output);
        }


        public static T Max<T>(this in NativeArray<T> source)
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

        public struct ArrayMaxFunc<T>
            : IFunc<NativeArray<T>, T>
            where T : struct, IComparable<T>
        {
            public T Invoke(NativeArray<T> arg0)
            {
                return arg0.Max();
            }
        }

        public static ValueFunc<NativeArray<T>, T>.Struct<ArrayMaxFunc<T>>
        MaxAsFunc<T>(
            this in NativeArray<T> source
            )
            where T : struct, IComparable<T>
        {
            return ValueFunc<NativeArray<T>, T>.New<ArrayMaxFunc<T>>();
        }

        public static T RunMax<T>(
            this in NativeArray<T> source
            )
            where T : struct, IComparable<T>
        {
            var func = source.MaxAsFunc();
            return source.Run(func);
        }

        public static JobHandle<T> ScheduleMax<T>(
            this in NativeArray<T> source
            )
            where T : struct, IComparable<T>
        {
            var func = source.MaxAsFunc();
            return source.Schedule(func);
        }

        public static JobHandle ScheduleMax<T>(
            this in NativeArray<T> source,
            ref NativeArray<T> output
            )
            where T : struct, IComparable<T>
        {
            var func = source.MaxAsFunc();
            return source.Schedule(func, ref output);
        }


        public static TResult Min<T, TResult, TSelector>(
            this in NativeArray<T> source,
            ValueFunc<T, TResult>.Struct<TSelector> selector
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

        public struct ArrayMinFunc<T, TResult, TSelector>
            : IFunc<NativeArray<T>, TResult>
            where T : struct
            where TResult : struct, IComparable<TResult>
            where TSelector : struct, IFunc<T, TResult>
        {
            public ValueFunc<T, TResult>.Struct<TSelector> Selector;

            public TResult Invoke(NativeArray<T> arg0)
            {
                return arg0.Min(Selector);
            }
        }

        public static ValueFunc<NativeArray<T>, TResult>.Struct<ArrayMinFunc<T, TResult, TSelector>>
        MinAsFunc<T, TResult, TSelector>(
            this in NativeArray<T> source,
            ValueFunc<T, TResult>.Struct<TSelector> selector
            )
            where T : struct
            where TResult : struct, IComparable<TResult>
            where TSelector : struct, IFunc<T, TResult>
        {
            var func = new ArrayMinFunc<T, TResult, TSelector> { Selector = selector };
            return ValueFunc<NativeArray<T>, TResult>.New(func);
        }

        public static TResult RunMin<T, TResult, TSelector>(
            this in NativeArray<T> source,
            ValueFunc<T, TResult>.Struct<TSelector> selector
            )
            where T : struct
            where TResult : struct, IComparable<TResult>
            where TSelector : struct, IFunc<T, TResult>
        {
            var func = source.MinAsFunc(selector);
            return source.Run(func);
        }

        public static JobHandle<TResult> ScheduleMin<T, TResult, TSelector>(
            this in NativeArray<T> source,
            ValueFunc<T, TResult>.Struct<TSelector> selector
            )
            where T : struct
            where TResult : struct, IComparable<TResult>
            where TSelector : struct, IFunc<T, TResult>
        {
            var func = source.MinAsFunc(selector);
            return source.Schedule(func);
        }

        public static JobHandle ScheduleMin<T, TResult, TSelector>(
            this in NativeArray<T> source,
            ref NativeArray<TResult> output,
            ValueFunc<T, TResult>.Struct<TSelector> selector
            )
            where T : struct
            where TResult : struct, IComparable<TResult>
            where TSelector : struct, IFunc<T, TResult>
        {
            var func = source.MinAsFunc(selector);
            return source.Schedule(func, ref output);
        }


        public static T Min<T>(this in NativeArray<T> source)
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

        public struct ArrayMinFunc<T>
            : IFunc<NativeArray<T>, T>
            where T : struct, IComparable<T>
        {
            public T Invoke(NativeArray<T> arg0)
            {
                return arg0.Min();
            }
        }

        public static ValueFunc<NativeArray<T>, T>.Struct<ArrayMinFunc<T>>
        MinAsFunc<T>(
            this in NativeArray<T> source
            )
            where T : struct, IComparable<T>
        {
            return ValueFunc<NativeArray<T>, T>.New<ArrayMinFunc<T>>();
        }

        public static T RunMin<T>(
            this in NativeArray<T> source
            )
            where T : struct, IComparable<T>
        {
            var func = source.MinAsFunc();
            return source.Run(func);
        }

        public static JobHandle<T> ScheduleMin<T>(
            this in NativeArray<T> source
            )
            where T : struct, IComparable<T>
        {
            var func = source.MinAsFunc();
            return source.Schedule(func);
        }

        public static JobHandle ScheduleMin<T>(
            this in NativeArray<T> source,
            ref NativeArray<T> output
            )
            where T : struct, IComparable<T>
        {
            var func = source.MinAsFunc();
            return source.Schedule(func, ref output);
        }
    }
}
