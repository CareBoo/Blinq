using System;
using System.Collections.Generic;
using CareBoo.Burst.Delegates;
using Unity.Collections;
using Unity.Jobs;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static bool Contains<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            in T item
            )
            where T : struct, IEquatable<T>
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var sourceList = source.ToNativeList(Allocator.Temp);
            for (var i = 0; i < sourceList.Length; i++)
                if (sourceList[i].Equals(item))
                {
                    sourceList.Dispose();
                    return true;
                }
            sourceList.Dispose();
            return false;
        }

        public struct SequenceContainsFunc<T, TSource, TSourceEnumerator>
            : IFunc<ValueSequence<T, TSource, TSourceEnumerator>, bool>
            where T : struct, IEquatable<T>
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            public T Item;

            public bool Invoke(ValueSequence<T, TSource, TSourceEnumerator> arg0)
            {
                return arg0.Contains(Item);
            }
        }

        public static ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, bool>.Struct<SequenceContainsFunc<T, TSource, TSourceEnumerator>>
        ContainsAsFunc<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            in T item
            )
            where T : struct, IEquatable<T>
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var funcStruct = new SequenceContainsFunc<T, TSource, TSourceEnumerator> { Item = item };
            return ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, bool>.New(funcStruct);
        }

        public static bool RunContains<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            in T item
            )
            where T : struct, IEquatable<T>
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var func = source.ContainsAsFunc(item);
            return source.Run(func);
        }

        public static JobHandle<bool> ScheduleContains<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            in T item
            )
            where T : struct, IEquatable<T>
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var func = source.ContainsAsFunc(item);
            return source.Schedule(func);
        }

        public static JobHandle ScheduleContains<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ref NativeArray<bool> output,
            in T item
            )
            where T : struct, IEquatable<T>
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var func = source.ContainsAsFunc(item);
            return source.Schedule(func, ref output);
        }

        public static bool Contains<T, TSource, TSourceEnumerator, TComparer>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            in T item,
            in TComparer comparer
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TComparer : struct, IEqualityComparer<T>
        {
            var sourceList = source.ToNativeList(Allocator.Temp);
            for (var i = 0; i < sourceList.Length; i++)
                if (comparer.Equals(sourceList[i], item))
                {
                    sourceList.Dispose();
                    return true;
                }
            sourceList.Dispose();
            return false;
        }

        public struct SequenceContainsFunc<T, TSource, TSourceEnumerator, TComparer>
            : IFunc<ValueSequence<T, TSource, TSourceEnumerator>, bool>
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TComparer : struct, IEqualityComparer<T>
        {
            public T Item;
            public TComparer Comparer;

            public bool Invoke(ValueSequence<T, TSource, TSourceEnumerator> seq)
            {
                return seq.Contains(Item, Comparer);
            }
        }

        public static ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, bool>.Struct<SequenceContainsFunc<T, TSource, TSourceEnumerator, TComparer>>
        ContainsAsFunc<T, TSource, TSourceEnumerator, TComparer>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            in T item,
            in TComparer comparer
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TComparer : struct, IEqualityComparer<T>
        {
            var funcStruct = new SequenceContainsFunc<T, TSource, TSourceEnumerator, TComparer> { Item = item, Comparer = comparer };
            return ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, bool>.New(funcStruct);
        }

        public static bool RunContains<T, TSource, TSourceEnumerator, TComparer>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            in T item,
            in TComparer comparer
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TComparer : struct, IEqualityComparer<T>
        {
            var func = source.ContainsAsFunc(item, comparer);
            return source.Run(func);
        }

        public static JobHandle<bool> ScheduleContains<T, TSource, TSourceEnumerator, TComparer>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            in T item,
            in TComparer comparer
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TComparer : struct, IEqualityComparer<T>
        {
            var func = source.ContainsAsFunc(item, comparer);
            return source.Schedule(func);
        }

        public static JobHandle ScheduleContains<T, TSource, TSourceEnumerator, TComparer>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ref NativeArray<bool> output,
            in T item,
            in TComparer comparer
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TComparer : struct, IEqualityComparer<T>
        {
            var func = source.ContainsAsFunc(item, comparer);
            return source.Schedule(func, ref output);
        }
    }
}
