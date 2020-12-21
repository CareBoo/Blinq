using CareBoo.Burst.Delegates;
using Unity.Collections;
using Unity.Jobs;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static T ElementAt<T>(
            this in NativeArray<T> source,
            int index
            )
            where T : struct
        {
            return source[index];
        }

        public struct ArrayElementAtFunc<T>
            : IFunc<NativeArray<T>, T>
            where T : struct
        {
            public int Index;

            public T Invoke(NativeArray<T> arg0)
            {
                return arg0.ElementAt(Index);
            }
        }

        public static ValueFunc<NativeArray<T>, T>.Struct<ArrayElementAtFunc<T>>
        ElementAtAsFunc<T>(
            this in NativeArray<T> source,
            int index
            )
            where T : struct
        {
            var func = new ArrayElementAtFunc<T> { Index = index };
            return ValueFunc<NativeArray<T>, T>.New(func);
        }

        public static T RunElementAt<T>(
            this in NativeArray<T> source,
            int index
            )
            where T : struct
        {
            var func = source.ElementAtAsFunc(index);
            return source.Run(func);
        }

        public static JobHandle ScheduleElementAt<T>(
            this in NativeArray<T> source,
            ref NativeArray<T> output,
            int index
            )
            where T : struct
        {
            var func = source.ElementAtAsFunc(index);
            return source.Schedule(func, ref output);
        }

        public static JobHandle<T> ScheduleElementAt<T>(
            this in NativeArray<T> source,
            int index
            )
            where T : struct
        {
            var func = source.ElementAtAsFunc(index);
            return source.Schedule(func);
        }

        public static T ElementAtOrDefault<T>(
            this in NativeArray<T> source,
            int index,
            in T defaultVal = default
            )
            where T : struct
        {
            if (index >= 0 && index < source.Length)
                return source[index];
            return defaultVal;
        }

        public struct ArrayElementAtOrDefaultFunc<T>
            : IFunc<NativeArray<T>, T>
            where T : struct
        {
            public int Index;
            public T Default;

            public T Invoke(NativeArray<T> arg0)
            {
                return arg0.ElementAtOrDefault(Index);
            }
        }

        public static ValueFunc<NativeArray<T>, T>.Struct<ArrayElementAtOrDefaultFunc<T>>
        ElementAtOrDefaultAsFunc<T>(
            this in NativeArray<T> source,
            int index,
            in T defaultVal = default
            )
            where T : struct
        {
            var func = new ArrayElementAtOrDefaultFunc<T> { Index = index, Default = defaultVal };
            return ValueFunc<NativeArray<T>, T>.New(func);
        }

        public static T RunElementAtOrDefault<T>(
            this in NativeArray<T> source,
            int index,
            in T defaultVal = default
            )
            where T : struct
        {
            var func = source.ElementAtOrDefaultAsFunc(index, in defaultVal);
            return source.Run(func);
        }

        public static JobHandle ScheduleElementAtOrDefault<T>(
            this in NativeArray<T> source,
            ref NativeArray<T> output,
            int index,
            in T defaultVal = default
            )
            where T : struct
        {
            var func = source.ElementAtOrDefaultAsFunc(index, in defaultVal);
            return source.Schedule(func, ref output);
        }

        public static JobHandle<T> ScheduleElementAtOrDefault<T>(
            this in NativeArray<T> source,
            int index,
            in T defaultVal = default
            )
            where T : struct
        {
            var func = source.ElementAtOrDefaultAsFunc(index, in defaultVal);
            return source.Schedule(func);
        }
    }
}
