using Unity.Collections;
using CareBoo.Burst.Delegates;
using Unity.Jobs;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static T Single<T, TPredicate>(
            this in NativeArray<T> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            T result = default;
            var isFound = false;
            for (var i = 0; i < source.Length; i++)
            {
                var val = source[i];
                if (predicate.Invoke(val))
                {
                    if (isFound)
                        throw Error.MoreThanOneMatch();
                    isFound = true;
                    result = val;
                }
            }
            if (isFound)
                return result;
            throw Error.NoMatch();
        }

        public struct ArraySingleFunc<T, TPredicate>
            : IFunc<NativeArray<T>, T>
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            public ValueFunc<T, bool>.Struct<TPredicate> Predicate;
            public T Invoke(NativeArray<T> arg0)
            {
                return arg0.Single(Predicate);
            }
        }

        public static ValueFunc<NativeArray<T>, T>.Struct<ArraySingleFunc<T, TPredicate>> SingleAsFunc<T, TPredicate>(
            this in NativeArray<T> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            var func = new ArraySingleFunc<T, TPredicate> { Predicate = predicate };
            return ValueFunc<NativeArray<T>, T>.New(func);
        }

        public static T RunSingle<T, TPredicate>(
            this in NativeArray<T> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            var ArraySingleFunc = source.SingleAsFunc(predicate);
            return source.Run(source.SingleAsFunc(predicate));
        }

        public static JobHandle ScheduleSingle<T, TPredicate>(
            this in NativeArray<T> source,
            ref NativeArray<T> output,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            var ArraySingleFunc = source.SingleAsFunc(predicate);
            return source.Schedule(source.SingleAsFunc(predicate), ref output);
        }

        public static JobHandle<T> ScheduleSingle<T, TPredicate>(
            this in NativeArray<T> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            var ArraySingleFunc = source.SingleAsFunc(predicate);
            return source.Schedule(source.SingleAsFunc(predicate));
        }

        public static T SingleOrDefault<T, TPredicate>(
            this in NativeArray<T> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate,
            in T defaultVal = default
            )
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            T result = default;
            bool isFound = false;
            for (var i = 0; i < source.Length; i++)
            {
                var val = source[i];
                if (predicate.Invoke(val))
                {
                    if (isFound)
                        throw Error.MoreThanOneMatch();
                    isFound = true;
                    result = val;
                }
            }
            return isFound ? result : defaultVal;
        }

        public struct ArraySingleOrDefaultFunc<T, TPredicate>
            : IFunc<NativeArray<T>, T>
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            public ValueFunc<T, bool>.Struct<TPredicate> Predicate;
            public T Default;
            public T Invoke(NativeArray<T> arg0)
            {
                return arg0.SingleOrDefault(Predicate, in Default);
            }
        }

        public static ValueFunc<NativeArray<T>, T>.Struct<ArraySingleOrDefaultFunc<T, TPredicate>> SingleOrDefaultAsFunc<T, TPredicate>(
            this in NativeArray<T> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate,
            in T defaultVal = default
            )
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            var func = new ArraySingleOrDefaultFunc<T, TPredicate> { Predicate = predicate, Default = defaultVal };
            return ValueFunc<NativeArray<T>, T>.New(func);
        }

        public static T RunSingleOrDefault<T, TPredicate>(
            this in NativeArray<T> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate,
            in T defaultVal = default
            )
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            var func = source.SingleOrDefaultAsFunc(predicate, in defaultVal);
            return source.Run(func);
        }

        public static JobHandle ScheduleSingleOrDefault<T, TPredicate>(
            this in NativeArray<T> source,
            ref NativeArray<T> output,
            ValueFunc<T, bool>.Struct<TPredicate> predicate,
            in T defaultVal = default
            )
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            var func = source.SingleOrDefaultAsFunc(predicate, in defaultVal);
            return source.Schedule(func, ref output);
        }

        public static JobHandle<T> ScheduleSingleOrDefault<T, TPredicate>(
            this in NativeArray<T> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate,
            in T defaultVal = default
            )
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            var func = source.SingleOrDefaultAsFunc(predicate, in defaultVal);
            return source.Schedule(func);
        }

        public static T Single<T>(
            this in NativeArray<T> source
            )
            where T : struct
        {
            if (source.Length > 1)
                throw Error.MoreThanOneElement();
            if (source.Length == 0)
                throw Error.NoElements();
            return source[0];
        }

        public struct ArraySingleFunc<T>
            : IFunc<NativeArray<T>, T>
            where T : struct
        {
            public T Invoke(NativeArray<T> arg0)
            {
                return arg0.Single();
            }
        }

        public static ValueFunc<NativeArray<T>, T>.Struct<ArraySingleFunc<T>> SingleAsFunc<T>(
            this in NativeArray<T> source
            )
            where T : struct
        {
            return default(ValueFunc<NativeArray<T>, T>.Struct<ArraySingleFunc<T>>);
        }

        public static T RunSingle<T>(
            this in NativeArray<T> source
            )
            where T : struct
        {
            var func = source.SingleAsFunc();
            return source.Run(func);
        }

        public static JobHandle ScheduleSingle<T>(
            this in NativeArray<T> source,
            ref NativeArray<T> output
            )
            where T : struct
        {
            var func = source.SingleAsFunc();
            return source.Schedule(func, ref output);
        }

        public static JobHandle<T> ScheduleSingle<T>(
            this in NativeArray<T> source
            )
            where T : struct
        {
            var func = source.SingleAsFunc();
            return source.Schedule(func);
        }

        public static T SingleOrDefault<T>(
            this in NativeArray<T> source,
            in T defaultVal = default
            )
            where T : struct
        {
            if (source.Length > 1)
                throw Error.MoreThanOneElement();
            if (source.Length == 0)
                return defaultVal;
            return source[0];
        }

        public struct ArraySingleOrDefaultFunc<T>
            : IFunc<NativeArray<T>, T>
            where T : struct
        {
            public T Default;
            public T Invoke(NativeArray<T> arg0)
            {
                return arg0.SingleOrDefault(in Default);
            }
        }

        public static ValueFunc<NativeArray<T>, T>.Struct<ArraySingleOrDefaultFunc<T>> SingleOrDefaultAsFunc<T>(
            this in NativeArray<T> source,
            in T defaultVal = default
            )
            where T : struct
        {
            var func = new ArraySingleOrDefaultFunc<T> { Default = defaultVal };
            return ValueFunc<NativeArray<T>, T>.New(func);
        }

        public static T RunSingleOrDefault<T>(
            this in NativeArray<T> source,
            in T defaultVal = default
            )
            where T : struct
        {
            var func = source.SingleOrDefaultAsFunc(in defaultVal);
            return source.Run(func);
        }

        public static JobHandle ScheduleSingleOrDefault<T>(
            this in NativeArray<T> source,
            ref NativeArray<T> output,
            in T defaultVal = default
            )
            where T : struct
        {
            var func = source.SingleOrDefaultAsFunc(in defaultVal);
            return source.Schedule(func, ref output);
        }

        public static JobHandle<T> ScheduleSingleOrDefault<T>(
            this in NativeArray<T> source,
            in T defaultVal = default
            )
            where T : struct
        {
            var func = source.SingleOrDefaultAsFunc(in defaultVal);
            return source.Schedule(func);
        }
    }
}
