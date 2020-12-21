using Unity.Collections;
using CareBoo.Burst.Delegates;
using Unity.Jobs;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static T Last<T, TPredicate>(
            this in NativeArray<T> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            for (var i = source.Length - 1; i > -1; i--)
            {
                var val = source[i];
                if (predicate.Invoke(val))
                    return val;
            }
            throw Error.NoMatch();
        }

        public struct ArrayLastFunc<T, TPredicate>
            : IFunc<NativeArray<T>, T>
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            public ValueFunc<T, bool>.Struct<TPredicate> Predicate;
            public T Invoke(NativeArray<T> arg0)
            {
                return arg0.Last(Predicate);
            }
        }

        public static ValueFunc<NativeArray<T>, T>.Struct<ArrayLastFunc<T, TPredicate>> LastAsFunc<T, TPredicate>(
            this in NativeArray<T> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            var func = new ArrayLastFunc<T, TPredicate> { Predicate = predicate };
            return ValueFunc<NativeArray<T>, T>.New(func);
        }

        public static T RunLast<T, TPredicate>(
            this in NativeArray<T> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            var ArrayLastFunc = source.LastAsFunc(predicate);
            return source.Run(source.LastAsFunc(predicate));
        }

        public static JobHandle ScheduleLast<T, TPredicate>(
            this in NativeArray<T> source,
            ref NativeArray<T> output,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            var ArrayLastFunc = source.LastAsFunc(predicate);
            return source.Schedule(source.LastAsFunc(predicate), ref output);
        }

        public static JobHandle<T> ScheduleLast<T, TPredicate>(
            this in NativeArray<T> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            var ArrayLastFunc = source.LastAsFunc(predicate);
            return source.Schedule(source.LastAsFunc(predicate));
        }

        public static T LastOrDefault<T, TPredicate>(
            this in NativeArray<T> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate,
            in T defaultVal = default
            )
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            for (var i = source.Length - 1; i > -1; i--)
            {
                var val = source[i];
                if (predicate.Invoke(val))
                    return val;
            }
            return defaultVal;
        }

        public struct ArrayLastOrDefaultFunc<T, TPredicate>
            : IFunc<NativeArray<T>, T>
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            public ValueFunc<T, bool>.Struct<TPredicate> Predicate;
            public T Default;
            public T Invoke(NativeArray<T> arg0)
            {
                return arg0.LastOrDefault(Predicate, in Default);
            }
        }

        public static ValueFunc<NativeArray<T>, T>.Struct<ArrayLastOrDefaultFunc<T, TPredicate>> LastOrDefaultAsFunc<T, TPredicate>(
            this in NativeArray<T> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate,
            in T defaultVal = default
            )
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            var func = new ArrayLastOrDefaultFunc<T, TPredicate> { Predicate = predicate, Default = defaultVal };
            return ValueFunc<NativeArray<T>, T>.New(func);
        }

        public static T RunLastOrDefault<T, TPredicate>(
            this in NativeArray<T> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate,
            in T defaultVal = default
            )
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            var func = source.LastOrDefaultAsFunc(predicate, in defaultVal);
            return source.Run(func);
        }

        public static JobHandle ScheduleLastOrDefault<T, TPredicate>(
            this in NativeArray<T> source,
            ref NativeArray<T> output,
            ValueFunc<T, bool>.Struct<TPredicate> predicate,
            in T defaultVal = default
            )
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            var func = source.LastOrDefaultAsFunc(predicate, in defaultVal);
            return source.Schedule(func, ref output);
        }

        public static JobHandle<T> ScheduleLastOrDefault<T, TPredicate>(
            this in NativeArray<T> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate,
            in T defaultVal = default
            )
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            var func = source.LastOrDefaultAsFunc(predicate, in defaultVal);
            return source.Schedule(func);
        }

        public static T Last<T>(
            this in NativeArray<T> source
            )
            where T : struct
        {
            return source[source.Length - 1];
        }

        public struct ArrayLastFunc<T>
            : IFunc<NativeArray<T>, T>
            where T : struct
        {
            public T Invoke(NativeArray<T> arg0)
            {
                return arg0.Last();
            }
        }

        public static ValueFunc<NativeArray<T>, T>.Struct<ArrayLastFunc<T>> LastAsFunc<T>(
            this in NativeArray<T> source
            )
            where T : struct
        {
            return default(ValueFunc<NativeArray<T>, T>.Struct<ArrayLastFunc<T>>);
        }

        public static T RunLast<T>(
            this in NativeArray<T> source
            )
            where T : struct
        {
            var func = source.LastAsFunc();
            return source.Run(func);
        }

        public static JobHandle ScheduleLast<T>(
            this in NativeArray<T> source,
            ref NativeArray<T> output
            )
            where T : struct
        {
            var func = source.LastAsFunc();
            return source.Schedule(func, ref output);
        }

        public static JobHandle<T> ScheduleLast<T>(
            this in NativeArray<T> source
            )
            where T : struct
        {
            var func = source.LastAsFunc();
            return source.Schedule(func);
        }

        public static T LastOrDefault<T>(
            this in NativeArray<T> source,
            in T defaultVal = default
            )
            where T : struct
        {
            if (source.Length == 0)
                return defaultVal;
            return source[source.Length - 1];
        }

        public struct ArrayLastOrDefaultFunc<T>
            : IFunc<NativeArray<T>, T>
            where T : struct
        {
            public T Default;
            public T Invoke(NativeArray<T> arg0)
            {
                return arg0.LastOrDefault(in Default);
            }
        }

        public static ValueFunc<NativeArray<T>, T>.Struct<ArrayLastOrDefaultFunc<T>> LastOrDefaultAsFunc<T>(
            this in NativeArray<T> source,
            in T defaultVal = default
            )
            where T : struct
        {
            var func = new ArrayLastOrDefaultFunc<T> { Default = defaultVal };
            return ValueFunc<NativeArray<T>, T>.New(func);
        }

        public static T RunLastOrDefault<T>(
            this in NativeArray<T> source,
            in T defaultVal = default
            )
            where T : struct
        {
            var func = source.LastOrDefaultAsFunc(in defaultVal);
            return source.Run(func);
        }

        public static JobHandle ScheduleLastOrDefault<T>(
            this in NativeArray<T> source,
            ref NativeArray<T> output,
            in T defaultVal = default
            )
            where T : struct
        {
            var func = source.LastOrDefaultAsFunc(in defaultVal);
            return source.Schedule(func, ref output);
        }

        public static JobHandle<T> ScheduleLastOrDefault<T>(
            this in NativeArray<T> source,
            in T defaultVal = default
            )
            where T : struct
        {
            var func = source.LastOrDefaultAsFunc(in defaultVal);
            return source.Schedule(func);
        }
    }
}
