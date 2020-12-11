using System.Collections.Generic;
using CareBoo.Burst.Delegates;
using Unity.Collections;
using Unity.Jobs;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {

        public static int Count<T, TSource, TSourceEnumerator, TPredicate>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var sourceList = source.ToNativeList(Allocator.Temp);
            var count = 0;
            for (var i = 0; i < sourceList.Length; i++)
                if (predicate.Invoke(sourceList[i]))
                    count++;
            sourceList.Dispose();
            return count;
        }

        public struct SequenceCountFunc<T, TSource, TSourceEnumerator, TPredicate>
            : IFunc<ValueSequence<T, TSource, TSourceEnumerator>, int>
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            public ValueFunc<T, bool>.Struct<TPredicate> Predicate;

            public int Invoke(ValueSequence<T, TSource, TSourceEnumerator> arg0)
            {
                return arg0.Count(Predicate);
            }
        }

        public static ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, int>.Struct<SequenceCountFunc<T, TSource, TSourceEnumerator, TPredicate>>
        NewSequenceCountFunc<T, TSource, TSourceEnumerator, TPredicate>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var funcStruct = new SequenceCountFunc<T, TSource, TSourceEnumerator, TPredicate> { Predicate = predicate };
            return ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, int>.New<SequenceCountFunc<T, TSource, TSourceEnumerator, TPredicate>>(funcStruct);
        }

        public static int RunCount<T, TSource, TSourceEnumerator, TPredicate>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var func = source.NewSequenceCountFunc(predicate);
            return source.Run(func);
        }

        public static JobHandle<int> ScheduleCount<T, TSource, TSourceEnumerator, TPredicate>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var func = source.NewSequenceCountFunc(predicate);
            return source.Schedule(func);
        }

        public static JobHandle ScheduleCount<T, TSource, TSourceEnumerator, TPredicate>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ref NativeArray<int> output,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var func = source.NewSequenceCountFunc(predicate);
            return source.Schedule(func, ref output);
        }

        public static int Count<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var sourceList = source.ToNativeList(Allocator.Temp);
            var result = sourceList.Length;
            sourceList.Dispose();
            return result;
        }

        public struct SequenceCountFunc<T, TSource, TSourceEnumerator>
            : IFunc<ValueSequence<T, TSource, TSourceEnumerator>, int>
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            public int Invoke(ValueSequence<T, TSource, TSourceEnumerator> arg0)
            {
                return arg0.Count();
            }
        }

        public static ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, int>.Struct<SequenceCountFunc<T, TSource, TSourceEnumerator>>
        NewSequenceCountFunc<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            return ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, int>.New<SequenceCountFunc<T, TSource, TSourceEnumerator>>();
        }

        public static int RunCount<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var func = source.NewSequenceCountFunc();
            return source.Run(func);
        }

        public static JobHandle<int> ScheduleCount<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var func = source.NewSequenceCountFunc();
            return source.Schedule(func);
        }

        public static JobHandle ScheduleCount<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ref NativeArray<int> output
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var func = source.NewSequenceCountFunc();
            return source.Schedule(func, ref output);
        }
    }
}
