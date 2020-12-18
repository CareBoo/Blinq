using System.Collections.Generic;
using CareBoo.Burst.Delegates;
using Unity.Collections;
using Unity.Jobs;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static bool Any<T, TSource, TSourceEnumerator, TPredicate>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var sourceList = source.ToNativeList(Allocator.Temp);
            for (var i = 0; i < sourceList.Length; i++)
                if (predicate.Invoke(sourceList[i]))
                {
                    sourceList.Dispose();
                    return true;
                }
            sourceList.Dispose();
            return false;
        }

        public struct SequenceAnyFunc<T, TSource, TSourceEnumerator, TPredicate>
            : IFunc<ValueSequence<T, TSource, TSourceEnumerator>, bool>
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            public ValueFunc<T, bool>.Struct<TPredicate> Predicate;

            public bool Invoke(ValueSequence<T, TSource, TSourceEnumerator> seq)
            {
                return seq.Any(Predicate);
            }
        }

        public static ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, bool>.Struct<SequenceAnyFunc<T, TSource, TSourceEnumerator, TPredicate>>
        AnyAsFunc<T, TSource, TSourceEnumerator, TPredicate>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var func = new SequenceAnyFunc<T, TSource, TSourceEnumerator, TPredicate> { Predicate = predicate };
            return ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, bool>.New(func);
        }

        public static bool RunAny<T, TSource, TSourceEnumerator, TPredicate>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var func = source.AnyAsFunc(predicate);
            return source.Run(func);
        }

        public static JobHandle<bool> ScheduleAny<T, TSource, TSourceEnumerator, TPredicate>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var func = source.AnyAsFunc(predicate);
            return source.Schedule(func);
        }

        public static JobHandle ScheduleAny<T, TSource, TSourceEnumerator, TPredicate>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ref NativeArray<bool> output,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var func = source.AnyAsFunc(predicate);
            return source.Schedule(func, ref output);
        }

        public static bool Any<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var sourceList = source.ToNativeList(Allocator.Temp);
            var result = sourceList.Length > 0;
            sourceList.Dispose();
            return result;
        }

        public struct SequenceAnyFunc<T, TSource, TSourceEnumerator>
            : IFunc<ValueSequence<T, TSource, TSourceEnumerator>, bool>
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            public bool Invoke(ValueSequence<T, TSource, TSourceEnumerator> seq)
            {
                return seq.Any();
            }
        }

        public static ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, bool>.Struct<SequenceAnyFunc<T, TSource, TSourceEnumerator>>
        AnyAsFunc<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            return ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, bool>.New<SequenceAnyFunc<T, TSource, TSourceEnumerator>>();
        }

        public static bool RunAny<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var func = source.AnyAsFunc();
            return source.Run(func);
        }

        public static JobHandle<bool> ScheduleAny<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var func = source.AnyAsFunc();
            return source.Schedule(func);
        }

        public static JobHandle ScheduleAny<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ref NativeArray<bool> output
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var func = source.AnyAsFunc();
            return source.Schedule(func, ref output);
        }
    }
}
