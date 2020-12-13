using System.Collections.Generic;
using CareBoo.Burst.Delegates;
using Unity.Collections;
using Unity.Jobs;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static T Last<T, TSource, TSourceEnumerator, TPredicate>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var list = source.ToNativeList(Allocator.Temp);
            for (var i = list.Length - 1; i > -1; i--)
            {
                var val = list[i];
                if (predicate.Invoke(val))
                {
                    list.Dispose();
                    return val;
                }
            }
            list.Dispose();
            throw Error.NoMatch();
        }

        public struct LastFunc<T, TSource, TSourceEnumerator, TPredicate>
            : IFunc<ValueSequence<T, TSource, TSourceEnumerator>, T>
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            public ValueFunc<T, bool>.Struct<TPredicate> Predicate;
            public T Invoke(ValueSequence<T, TSource, TSourceEnumerator> arg0)
            {
                return arg0.Last(Predicate);
            }
        }

        public static ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, T>.Struct<LastFunc<T, TSource, TSourceEnumerator, TPredicate>> LastAsFunc<T, TSource, TSourceEnumerator, TPredicate>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var func = new LastFunc<T, TSource, TSourceEnumerator, TPredicate> { Predicate = predicate };
            return ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, T>.New(func);
        }

        public static T RunLast<T, TSource, TSourceEnumerator, TPredicate>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var singleFunc = source.LastAsFunc(predicate);
            return source.Run(source.LastAsFunc(predicate));
        }

        public static JobHandle ScheduleLast<T, TSource, TSourceEnumerator, TPredicate>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ref NativeArray<T> output,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var singleFunc = source.LastAsFunc(predicate);
            return source.Schedule(source.LastAsFunc(predicate), ref output);
        }

        public static JobHandle<T> ScheduleLast<T, TSource, TSourceEnumerator, TPredicate>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var singleFunc = source.LastAsFunc(predicate);
            return source.Schedule(source.LastAsFunc(predicate));
        }

        public static T LastOrDefault<T, TSource, TSourceEnumerator, TPredicate>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate,
            in T defaultVal = default
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var list = source.ToNativeList(Allocator.Temp);
            for (var i = list.Length - 1; i > -1; i--)
            {
                var val = list[i];
                if (predicate.Invoke(val))
                {
                    list.Dispose();
                    return val;
                }
            }
            list.Dispose();
            return defaultVal;
        }

        public struct LastOrDefaultFunc<T, TSource, TSourceEnumerator, TPredicate>
            : IFunc<ValueSequence<T, TSource, TSourceEnumerator>, T>
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            public ValueFunc<T, bool>.Struct<TPredicate> Predicate;
            public T Default;
            public T Invoke(ValueSequence<T, TSource, TSourceEnumerator> arg0)
            {
                return arg0.LastOrDefault(Predicate, in Default);
            }
        }

        public static ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, T>.Struct<LastOrDefaultFunc<T, TSource, TSourceEnumerator, TPredicate>> LastOrDefaultAsFunc<T, TSource, TSourceEnumerator, TPredicate>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate,
            in T defaultVal = default
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var func = new LastOrDefaultFunc<T, TSource, TSourceEnumerator, TPredicate> { Predicate = predicate, Default = defaultVal };
            return ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, T>.New(func);
        }

        public static T RunLastOrDefault<T, TSource, TSourceEnumerator, TPredicate>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate,
            in T defaultVal = default
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var func = source.LastOrDefaultAsFunc(predicate, in defaultVal);
            return source.Run(func);
        }

        public static JobHandle ScheduleLastOrDefault<T, TSource, TSourceEnumerator, TPredicate>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ref NativeArray<T> output,
            ValueFunc<T, bool>.Struct<TPredicate> predicate,
            in T defaultVal = default
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var func = source.LastOrDefaultAsFunc(predicate, in defaultVal);
            return source.Schedule(func, ref output);
        }

        public static JobHandle<T> ScheduleLastOrDefault<T, TSource, TSourceEnumerator, TPredicate>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate,
            in T defaultVal = default
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var func = source.LastOrDefaultAsFunc(predicate, in defaultVal);
            return source.Schedule(func);
        }

        public static T Last<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var list = source.ToNativeList(Allocator.Temp);
            if (list.Length == 0)
            {
                list.Dispose();
                throw Error.NoElements();
            }
            var result = list[list.Length - 1];
            list.Dispose();
            return result;
        }

        public struct LastFunc<T, TSource, TSourceEnumerator>
            : IFunc<ValueSequence<T, TSource, TSourceEnumerator>, T>
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            public T Invoke(ValueSequence<T, TSource, TSourceEnumerator> arg0)
            {
                return arg0.Last();
            }
        }

        public static ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, T>.Struct<LastFunc<T, TSource, TSourceEnumerator>> LastAsFunc<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            return default(ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, T>.Struct<LastFunc<T, TSource, TSourceEnumerator>>);
        }

        public static T RunLast<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var func = source.LastAsFunc();
            return source.Run(func);
        }

        public static JobHandle ScheduleLast<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ref NativeArray<T> output
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var func = source.LastAsFunc();
            return source.Schedule(func, ref output);
        }

        public static JobHandle<T> ScheduleLast<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var func = source.LastAsFunc();
            return source.Schedule(func);
        }


        public static T LastOrDefault<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            in T defaultVal = default
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var list = source.ToNativeList(Allocator.Temp);
            if (list.Length == 0)
            {
                list.Dispose();
                return defaultVal;
            }
            var result = list[list.Length - 1];
            list.Dispose();
            return result;
        }

        public struct LastOrDefaultFunc<T, TSource, TSourceEnumerator>
            : IFunc<ValueSequence<T, TSource, TSourceEnumerator>, T>
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            public T Default;
            public T Invoke(ValueSequence<T, TSource, TSourceEnumerator> arg0)
            {
                return arg0.LastOrDefault(in Default);
            }
        }

        public static ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, T>.Struct<LastOrDefaultFunc<T, TSource, TSourceEnumerator>> LastOrDefaultAsFunc<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            in T defaultVal = default
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var func = new LastOrDefaultFunc<T, TSource, TSourceEnumerator> { Default = defaultVal };
            return ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, T>.New(func);
        }

        public static T RunLastOrDefault<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            in T defaultVal = default
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var func = source.LastOrDefaultAsFunc(in defaultVal);
            return source.Run(func);
        }

        public static JobHandle ScheduleLastOrDefault<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ref NativeArray<T> output,
            in T defaultVal = default
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var func = source.LastOrDefaultAsFunc(in defaultVal);
            return source.Schedule(func, ref output);
        }

        public static JobHandle<T> ScheduleLastOrDefault<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            in T defaultVal = default
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var func = source.LastOrDefaultAsFunc(in defaultVal);
            return source.Schedule(func);
        }
    }
}
