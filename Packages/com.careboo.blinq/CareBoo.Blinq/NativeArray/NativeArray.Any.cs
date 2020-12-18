using Unity.Collections;
using CareBoo.Burst.Delegates;
using Unity.Jobs;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static bool Any<T, TPredicate>(
            this in NativeArray<T> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            for (var i = 0; i < source.Length; i++)
                if (predicate.Invoke(source[i]))
                    return true;
            return false;
        }

        public struct ArrayAnyFunc<T, TPredicate>
            : IFunc<NativeArray<T>, bool>
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            public ValueFunc<T, bool>.Struct<TPredicate> Predicate;

            public bool Invoke(NativeArray<T> seq)
            {
                return seq.Any(Predicate);
            }
        }

        public static ValueFunc<NativeArray<T>, bool>.Struct<ArrayAnyFunc<T, TPredicate>>
        AnyAsFunc<T, TPredicate>(
            this in NativeArray<T> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            var func = new ArrayAnyFunc<T, TPredicate> { Predicate = predicate };
            return ValueFunc<NativeArray<T>, bool>.New(func);
        }

        public static bool RunAny<T, TPredicate>(
            this in NativeArray<T> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            var func = source.AnyAsFunc(predicate);
            return source.Run(func);
        }

        public static JobHandle<bool> ScheduleAny<T, TPredicate>(
            this in NativeArray<T> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            var func = source.AnyAsFunc(predicate);
            return source.Schedule(func);
        }

        public static JobHandle ScheduleAny<T, TPredicate>(
            this in NativeArray<T> source,
            ref NativeArray<bool> output,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            var func = source.AnyAsFunc(predicate);
            return source.Schedule(func, ref output);
        }

        public static bool Any<T>(this in NativeArray<T> source)
            where T : struct
        {
            return source.Length > 0;
        }

        public struct ArrayAnyFunc<T>
            : IFunc<NativeArray<T>, bool>
            where T : struct
        {
            public bool Invoke(NativeArray<T> seq)
            {
                return seq.Any();
            }
        }

        public static ValueFunc<NativeArray<T>, bool>.Struct<ArrayAnyFunc<T>>
        AnyAsFunc<T>(
            this in NativeArray<T> source
            )
            where T : struct
        {
            return ValueFunc<NativeArray<T>, bool>.New<ArrayAnyFunc<T>>();
        }

        public static bool RunAny<T>(
            this in NativeArray<T> source
            )
            where T : struct
        {
            var func = source.AnyAsFunc();
            return source.Run(func);
        }

        public static JobHandle<bool> ScheduleAny<T>(
            this in NativeArray<T> source
            )
            where T : struct
        {
            var func = source.AnyAsFunc();
            return source.Schedule(func);
        }

        public static JobHandle ScheduleAny<T>(
            this in NativeArray<T> source,
            ref NativeArray<bool> output
            )
            where T : struct
        {
            var func = source.AnyAsFunc();
            return source.Schedule(func, ref output);
        }
    }
}
