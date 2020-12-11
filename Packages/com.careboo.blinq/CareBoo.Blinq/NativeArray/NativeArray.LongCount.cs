using Unity.Collections;
using CareBoo.Burst.Delegates;
using Unity.Jobs;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static long LongCount<T, TPredicate>(
            this in NativeArray<T> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            var count = 0;
            for (var i = 0; i < source.Length; i++)
                if (predicate.Invoke(source[i]))
                    count++;
            return count;
        }

        public struct ArrayLongCountFunc<T, TPredicate>
            : IFunc<NativeArray<T>, int>
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            public ValueFunc<T, bool>.Struct<TPredicate> Predicate;

            public int Invoke(NativeArray<T> arg0)
            {
                return arg0.Count(Predicate);
            }
        }

        public static ValueFunc<NativeArray<T>, int>.Struct<ArrayLongCountFunc<T, TPredicate>>
        NewArrayLongCountFunc<T, TPredicate>(
            this in NativeArray<T> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            var funcStruct = new ArrayLongCountFunc<T, TPredicate> { Predicate = predicate };
            return ValueFunc<NativeArray<T>, int>.New<ArrayLongCountFunc<T, TPredicate>>(funcStruct);
        }

        public static int RunLongCount<T, TPredicate>(
            this in NativeArray<T> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            var func = source.NewArrayLongCountFunc(predicate);
            return source.Run(func);
        }

        public static JobHandle<int> ScheduleLongCount<T, TPredicate>(
            this in NativeArray<T> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            var func = source.NewArrayLongCountFunc(predicate);
            return source.Schedule(func);
        }

        public static JobHandle ScheduleLongCount<T, TPredicate>(
            this in NativeArray<T> source,
            ref NativeArray<int> output,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            var func = source.NewArrayLongCountFunc(predicate);
            return source.Schedule(func, ref output);
        }

        public static long LongCount<T>(this in NativeArray<T> source)
            where T : struct
        {
            return source.Length;
        }

        public struct ArrayLongCountFunc<T>
            : IFunc<NativeArray<T>, int>
            where T : struct
        {
            public int Invoke(NativeArray<T> arg0)
            {
                return arg0.Count();
            }
        }

        public static ValueFunc<NativeArray<T>, int>.Struct<ArrayLongCountFunc<T>>
        NewArrayLongCountFunc<T>(
            this in NativeArray<T> source
            )
            where T : struct
        {
            return ValueFunc<NativeArray<T>, int>.New<ArrayLongCountFunc<T>>();
        }

        public static int RunLongCount<T>(
            this in NativeArray<T> source
            )
            where T : struct
        {
            var func = source.NewArrayLongCountFunc();
            return source.Run(func);
        }

        public static JobHandle<int> ScheduleLongCount<T>(
            this in NativeArray<T> source
            )
            where T : struct
        {
            var func = source.NewArrayLongCountFunc();
            return source.Schedule(func);
        }

        public static JobHandle ScheduleLongCount<T>(
            this in NativeArray<T> source,
            ref NativeArray<int> output
            )
            where T : struct
        {
            var func = source.NewArrayLongCountFunc();
            return source.Schedule(func, ref output);
        }
    }
}
