using Unity.Collections;
using CareBoo.Burst.Delegates;
using Unity.Jobs;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static int Count<T, TPredicate>(
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

        public struct SequenceCountFunc<T, TPredicate>
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

        public static ValueFunc<NativeArray<T>, int>.Struct<SequenceCountFunc<T, TPredicate>>
        NewSequenceCountFunc<T, TPredicate>(
            this in NativeArray<T> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            var funcStruct = new SequenceCountFunc<T, TPredicate> { Predicate = predicate };
            return ValueFunc<NativeArray<T>, int>.New<SequenceCountFunc<T, TPredicate>>(funcStruct);
        }

        public static int RunCount<T, TPredicate>(
            this in NativeArray<T> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            var func = source.NewSequenceCountFunc(predicate);
            return source.Run(func);
        }

        public static JobHandle<int> ScheduleCount<T, TPredicate>(
            this in NativeArray<T> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            var func = source.NewSequenceCountFunc(predicate);
            return source.Schedule(func);
        }

        public static JobHandle ScheduleCount<T, TPredicate>(
            this in NativeArray<T> source,
            ref NativeArray<int> output,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            var func = source.NewSequenceCountFunc(predicate);
            return source.Schedule(func, ref output);
        }

        public static int Count<T>(this in NativeArray<T> source)
            where T : struct
        {
            return source.Length;
        }

        public struct SequenceCountFunc<T>
            : IFunc<NativeArray<T>, int>
            where T : struct
        {
            public int Invoke(NativeArray<T> arg0)
            {
                return arg0.Count();
            }
        }

        public static ValueFunc<NativeArray<T>, int>.Struct<SequenceCountFunc<T>>
        NewSequenceCountFunc<T>(
            this in NativeArray<T> source
            )
            where T : struct
        {
            return ValueFunc<NativeArray<T>, int>.New<SequenceCountFunc<T>>();
        }

        public static int RunCount<T>(
            this in NativeArray<T> source
            )
            where T : struct
        {
            var func = source.NewSequenceCountFunc();
            return source.Run(func);
        }

        public static JobHandle<int> ScheduleCount<T>(
            this in NativeArray<T> source
            )
            where T : struct
        {
            var func = source.NewSequenceCountFunc();
            return source.Schedule(func);
        }

        public static JobHandle ScheduleCount<T>(
            this in NativeArray<T> source,
            ref NativeArray<int> output
            )
            where T : struct
        {
            var func = source.NewSequenceCountFunc();
            return source.Schedule(func, ref output);
        }
    }
}
