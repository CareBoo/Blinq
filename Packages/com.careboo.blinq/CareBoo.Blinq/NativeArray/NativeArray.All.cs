using Unity.Collections;
using CareBoo.Burst.Delegates;
using Unity.Jobs;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static bool All<T, TPredicate>(
            this in NativeArray<T> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            for (var i = 0; i < source.Length; i++)
                if (!predicate.Invoke(source[i]))
                    return false;
            return true;
        }

        public struct ArrayAllFunc<T, TPredicate>
            : IFunc<NativeArray<T>, bool>
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            public ValueFunc<T, bool>.Struct<TPredicate> Predicate;

            public bool Invoke(NativeArray<T> seq)
            {
                return seq.All(Predicate);
            }
        }

        public static ValueFunc<NativeArray<T>, bool>.Struct<ArrayAllFunc<T, TPredicate>>
        AllAsFunc<T, TPredicate>(
            this in NativeArray<T> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            var allFunc = new ArrayAllFunc<T, TPredicate> { Predicate = predicate };
            return ValueFunc<NativeArray<T>, bool>.New(allFunc);
        }

        public static bool RunAll<T, TPredicate>(
            this in NativeArray<T> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            var allFunc = source.AllAsFunc(predicate);
            return source.Run(allFunc);
        }

        public static JobHandle<bool> ScheduleAll<T, TPredicate>(
            this in NativeArray<T> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            var allFunc = source.AllAsFunc(predicate);
            return source.Schedule(allFunc);
        }

        public static JobHandle ScheduleAll<T, TPredicate>(
            this in NativeArray<T> source,
            ref NativeArray<bool> output,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            var allFunc = source.AllAsFunc(predicate);
            return source.Schedule(allFunc, ref output);
        }
    }
}
