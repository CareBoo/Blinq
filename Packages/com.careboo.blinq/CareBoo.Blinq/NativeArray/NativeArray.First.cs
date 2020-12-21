using Unity.Collections;
using CareBoo.Burst.Delegates;
using Unity.Jobs;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static T First<T, TPredicate>(
            this in NativeArray<T> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            for (var i = 0; i < source.Length; i++)
            {
                var val = source[i];
                if (predicate.Invoke(val))
                    return val;
            }
            throw Error.NoMatch();
        }

        public struct ArrayFirstFunc<T, TPredicate>
            : IFunc<NativeArray<T>, T>
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            public ValueFunc<T, bool>.Struct<TPredicate> Predicate;
            public T Invoke(NativeArray<T> arg0)
            {
                return arg0.First(Predicate);
            }
        }

        public static ValueFunc<NativeArray<T>, T>.Struct<ArrayFirstFunc<T, TPredicate>> FirstAsFunc<T, TPredicate>(
            this in NativeArray<T> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            var func = new ArrayFirstFunc<T, TPredicate> { Predicate = predicate };
            return ValueFunc<NativeArray<T>, T>.New(func);
        }

        public static T RunFirst<T, TPredicate>(
            this in NativeArray<T> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            var ArrayFirstFunc = source.FirstAsFunc(predicate);
            return source.Run(source.FirstAsFunc(predicate));
        }

        public static JobHandle ScheduleFirst<T, TPredicate>(
            this in NativeArray<T> source,
            ref NativeArray<T> output,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            var ArrayFirstFunc = source.FirstAsFunc(predicate);
            return source.Schedule(source.FirstAsFunc(predicate), ref output);
        }

        public static JobHandle<T> ScheduleFirst<T, TPredicate>(
            this in NativeArray<T> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            var ArrayFirstFunc = source.FirstAsFunc(predicate);
            return source.Schedule(source.FirstAsFunc(predicate));
        }


        public static T FirstOrDefault<T, TPredicate>(
            this in NativeArray<T> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate,
            in T defaultVal = default
            )
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            for (var i = 0; i < source.Length; i++)
            {
                var val = source[i];
                if (predicate.Invoke(val))
                    return val;
            }
            return defaultVal;
        }

        public struct ArrayFirstOrDefaultFunc<T, TPredicate>
            : IFunc<NativeArray<T>, T>
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            public ValueFunc<T, bool>.Struct<TPredicate> Predicate;
            public T Default;
            public T Invoke(NativeArray<T> arg0)
            {
                return arg0.FirstOrDefault(Predicate, in Default);
            }
        }

        public static ValueFunc<NativeArray<T>, T>.Struct<ArrayFirstOrDefaultFunc<T, TPredicate>> FirstOrDefaultAsFunc<T, TPredicate>(
            this in NativeArray<T> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate,
            in T defaultVal = default
            )
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            var func = new ArrayFirstOrDefaultFunc<T, TPredicate> { Predicate = predicate, Default = defaultVal };
            return ValueFunc<NativeArray<T>, T>.New(func);
        }

        public static T RunFirstOrDefault<T, TPredicate>(
            this in NativeArray<T> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate,
            in T defaultVal = default
            )
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            var func = source.FirstOrDefaultAsFunc(predicate, in defaultVal);
            return source.Run(func);
        }

        public static JobHandle ScheduleFirstOrDefault<T, TPredicate>(
            this in NativeArray<T> source,
            ref NativeArray<T> output,
            ValueFunc<T, bool>.Struct<TPredicate> predicate,
            in T defaultVal = default
            )
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            var func = source.FirstOrDefaultAsFunc(predicate, in defaultVal);
            return source.Schedule(func, ref output);
        }

        public static JobHandle<T> ScheduleFirstOrDefault<T, TPredicate>(
            this in NativeArray<T> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate,
            in T defaultVal = default
            )
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            var func = source.FirstOrDefaultAsFunc(predicate, in defaultVal);
            return source.Schedule(func);
        }

        public static T First<T>(
            this in NativeArray<T> source
            )
            where T : struct
        {
            return source[0];
        }

        public struct ArrayFirstFunc<T>
            : IFunc<NativeArray<T>, T>
            where T : struct
        {
            public T Invoke(NativeArray<T> arg0)
            {
                return arg0.First();
            }
        }

        public static ValueFunc<NativeArray<T>, T>.Struct<ArrayFirstFunc<T>> FirstAsFunc<T>(
            this in NativeArray<T> source
            )
            where T : struct
        {
            return default(ValueFunc<NativeArray<T>, T>.Struct<ArrayFirstFunc<T>>);
        }

        public static T RunFirst<T>(
            this in NativeArray<T> source
            )
            where T : struct
        {
            var func = source.FirstAsFunc();
            return source.Run(func);
        }

        public static JobHandle ScheduleFirst<T>(
            this in NativeArray<T> source,
            ref NativeArray<T> output
            )
            where T : struct
        {
            var func = source.FirstAsFunc();
            return source.Schedule(func, ref output);
        }

        public static JobHandle<T> ScheduleFirst<T>(
            this in NativeArray<T> source
            )
            where T : struct
        {
            var func = source.FirstAsFunc();
            return source.Schedule(func);
        }

        public static T FirstOrDefault<T>(
            this in NativeArray<T> source,
            in T defaultVal = default
            )
            where T : struct
        {
            if (source.Length == 0)
                return defaultVal;
            return source[0];
        }

        public struct ArrayFirstOrDefaultFunc<T>
            : IFunc<NativeArray<T>, T>
            where T : struct
        {
            public T Default;
            public T Invoke(NativeArray<T> arg0)
            {
                return arg0.FirstOrDefault(in Default);
            }
        }

        public static ValueFunc<NativeArray<T>, T>.Struct<ArrayFirstOrDefaultFunc<T>> FirstOrDefaultAsFunc<T>(
            this in NativeArray<T> source,
            in T defaultVal = default
            )
            where T : struct
        {
            var func = new ArrayFirstOrDefaultFunc<T> { Default = defaultVal };
            return ValueFunc<NativeArray<T>, T>.New(func);
        }

        public static T RunFirstOrDefault<T>(
            this in NativeArray<T> source,
            in T defaultVal = default
            )
            where T : struct
        {
            var func = source.FirstOrDefaultAsFunc(in defaultVal);
            return source.Run(func);
        }

        public static JobHandle ScheduleFirstOrDefault<T>(
            this in NativeArray<T> source,
            ref NativeArray<T> output,
            in T defaultVal = default
            )
            where T : struct
        {
            var func = source.FirstOrDefaultAsFunc(in defaultVal);
            return source.Schedule(func, ref output);
        }

        public static JobHandle<T> ScheduleFirstOrDefault<T>(
            this in NativeArray<T> source,
            in T defaultVal = default
            )
            where T : struct
        {
            var func = source.FirstOrDefaultAsFunc(in defaultVal);
            return source.Schedule(func);
        }
    }
}
