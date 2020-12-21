using System.Collections.Generic;
using CareBoo.Burst.Delegates;
using Unity.Collections;
using Unity.Jobs;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static T ElementAt<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            int index
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var list = source.ToNativeList(Allocator.Temp);
            var result = list[index];
            list.Dispose();
            return result;
        }

        public struct SequenceElementAtFunc<T, TSource, TSourceEnumerator>
            : IFunc<ValueSequence<T, TSource, TSourceEnumerator>, T>
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            public int Index;

            public T Invoke(ValueSequence<T, TSource, TSourceEnumerator> arg0)
            {
                return arg0.ElementAt(Index);
            }
        }

        public static ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, T>.Struct<SequenceElementAtFunc<T, TSource, TSourceEnumerator>>
        ElementAtAsFunc<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            int index
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var func = new SequenceElementAtFunc<T, TSource, TSourceEnumerator> { Index = index };
            return ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, T>.New(func);
        }

        public static T RunElementAt<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            int index
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var func = source.ElementAtAsFunc(index);
            return source.Run(func);
        }

        public static JobHandle ScheduleElementAt<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ref NativeArray<T> output,
            int index
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var func = source.ElementAtAsFunc(index);
            return source.Schedule(func, ref output);
        }

        public static JobHandle<T> ScheduleElementAt<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            int index
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var func = source.ElementAtAsFunc(index);
            return source.Schedule(func);
        }

        public static T ElementAtOrDefault<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            int index,
            in T defaultVal = default
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var list = source.ToNativeList(Allocator.Temp);
            var result = index >= 0 && index < list.Length
                ? list[index]
                : defaultVal;
            list.Dispose();
            return result;
        }

        public struct SequenceElementAtOrDefaultFunc<T, TSource, TSourceEnumerator>
            : IFunc<ValueSequence<T, TSource, TSourceEnumerator>, T>
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            public int Index;
            public T Default;

            public T Invoke(ValueSequence<T, TSource, TSourceEnumerator> arg0)
            {
                return arg0.ElementAtOrDefault(Index);
            }
        }

        public static ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, T>.Struct<SequenceElementAtOrDefaultFunc<T, TSource, TSourceEnumerator>>
        ElementAtOrDefaultAsFunc<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            int index,
            in T defaultVal = default
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var func = new SequenceElementAtOrDefaultFunc<T, TSource, TSourceEnumerator> { Index = index, Default = defaultVal };
            return ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, T>.New(func);
        }

        public static T RunElementAtOrDefault<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            int index,
            in T defaultVal = default
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var func = source.ElementAtOrDefaultAsFunc(index, in defaultVal);
            return source.Run(func);
        }

        public static JobHandle ScheduleElementAtOrDefault<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ref NativeArray<T> output,
            int index,
            in T defaultVal = default
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var func = source.ElementAtOrDefaultAsFunc(index, in defaultVal);
            return source.Schedule(func, ref output);
        }

        public static JobHandle<T> ScheduleElementAtOrDefault<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            int index,
            in T defaultVal = default
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var func = source.ElementAtOrDefaultAsFunc(index, in defaultVal);
            return source.Schedule(func);
        }
    }
}
