using System.Collections.Generic;
using CareBoo.Burst.Delegates;
using Unity.Collections;
using Unity.Jobs;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static long LongCount<T, TSource, TSourceEnumerator, TPredicate>(
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

        public struct SequenceLongCountFunc<T, TSource, TSourceEnumerator, TPredicate>
            : IFunc<ValueSequence<T, TSource, TSourceEnumerator>, long>
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            public ValueFunc<T, bool>.Struct<TPredicate> Predicate;

            public long Invoke(ValueSequence<T, TSource, TSourceEnumerator> arg0)
            {
                return arg0.Count(Predicate);
            }
        }

        public static ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, long>.Struct<SequenceLongCountFunc<T, TSource, TSourceEnumerator, TPredicate>>
        NewSequenceLongCountFunc<T, TSource, TSourceEnumerator, TPredicate>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var funcStruct = new SequenceLongCountFunc<T, TSource, TSourceEnumerator, TPredicate> { Predicate = predicate };
            return ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, long>.New<SequenceLongCountFunc<T, TSource, TSourceEnumerator, TPredicate>>(funcStruct);
        }

        public static long RunLongCount<T, TSource, TSourceEnumerator, TPredicate>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var func = source.NewSequenceLongCountFunc(predicate);
            return source.Run(func);
        }

        public static JobHandle<long> ScheduleLongCount<T, TSource, TSourceEnumerator, TPredicate>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var func = source.NewSequenceLongCountFunc(predicate);
            return source.Schedule(func);
        }

        public static JobHandle ScheduleLongCount<T, TSource, TSourceEnumerator, TPredicate>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ref NativeArray<long> output,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var func = source.NewSequenceLongCountFunc(predicate);
            return source.Schedule(func, ref output);
        }

        public static long LongCount<T, TSource, TSourceEnumerator>(
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

        public struct SequenceLongCountFunc<T, TSource, TSourceEnumerator>
            : IFunc<ValueSequence<T, TSource, TSourceEnumerator>, long>
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            public long Invoke(ValueSequence<T, TSource, TSourceEnumerator> arg0)
            {
                return arg0.Count();
            }
        }

        public static ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, long>.Struct<SequenceLongCountFunc<T, TSource, TSourceEnumerator>>
        NewSequenceLongCountFunc<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            return ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, long>.New<SequenceLongCountFunc<T, TSource, TSourceEnumerator>>();
        }

        public static long RunLongCount<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var func = source.NewSequenceLongCountFunc();
            return source.Run(func);
        }

        public static JobHandle<long> ScheduleLongCount<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var func = source.NewSequenceLongCountFunc();
            return source.Schedule(func);
        }

        public static JobHandle ScheduleLongCount<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ref NativeArray<long> output
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var func = source.NewSequenceLongCountFunc();
            return source.Schedule(func, ref output);
        }
    }
}
