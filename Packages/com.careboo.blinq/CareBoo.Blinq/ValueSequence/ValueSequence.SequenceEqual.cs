using System;
using System.Collections.Generic;
using CareBoo.Burst.Delegates;
using Unity.Collections;
using Unity.Jobs;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static bool SequenceEqual<T, TSource, TSourceEnumerator, TSecond, TSecondEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            in ValueSequence<T, TSecond, TSecondEnumerator> second
            )
            where T : struct, IEquatable<T>
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TSecond : struct, ISequence<T, TSecondEnumerator>
            where TSecondEnumerator : struct, IEnumerator<T>
        {
            var sourceList = source.ToNativeList(Allocator.Temp);
            var secondList = second.ToNativeList(Allocator.Temp);
            var result = sourceList.AsArray().ArraysEqual(secondList);
            sourceList.Dispose();
            secondList.Dispose();
            return result;
        }

        public struct SequenceSequenceEqualFunc<T, TSource, TSourceEnumerator, TSecond, TSecondEnumerator>
            : IFunc<ValueSequence<T, TSource, TSourceEnumerator>, bool>
            where T : struct, IEquatable<T>
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TSecond : struct, ISequence<T, TSecondEnumerator>
            where TSecondEnumerator : struct, IEnumerator<T>
        {
            public ValueSequence<T, TSecond, TSecondEnumerator> Second;

            public bool Invoke(ValueSequence<T, TSource, TSourceEnumerator> source)
            {
                return source.SequenceEqual(Second);
            }
        }

        public static ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, bool>.Struct<SequenceSequenceEqualFunc<T, TSource, TSourceEnumerator, TSecond, TSecondEnumerator>>
        SequenceEqualAsFunc<T, TSource, TSourceEnumerator, TSecond, TSecondEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            in ValueSequence<T, TSecond, TSecondEnumerator> second
            )
            where T : struct, IEquatable<T>
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TSecond : struct, ISequence<T, TSecondEnumerator>
            where TSecondEnumerator : struct, IEnumerator<T>
        {
            var func = new SequenceSequenceEqualFunc<T, TSource, TSourceEnumerator, TSecond, TSecondEnumerator> { Second = second };
            return ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, bool>.New(func);
        }

        public static bool RunSequenceEqual<T, TSource, TSourceEnumerator, TSecond, TSecondEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            in ValueSequence<T, TSecond, TSecondEnumerator> second
            )
            where T : struct, IEquatable<T>
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TSecond : struct, ISequence<T, TSecondEnumerator>
            where TSecondEnumerator : struct, IEnumerator<T>
        {
            var func = source.SequenceEqualAsFunc(second);
            return source.Run(func);
        }

        public static JobHandle<bool> ScheduleSequenceEqual<T, TSource, TSourceEnumerator, TSecond, TSecondEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            in ValueSequence<T, TSecond, TSecondEnumerator> second
            )
            where T : struct, IEquatable<T>
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TSecond : struct, ISequence<T, TSecondEnumerator>
            where TSecondEnumerator : struct, IEnumerator<T>
        {
            var func = source.SequenceEqualAsFunc(second);
            return source.Schedule(func);
        }

        public static JobHandle ScheduleSequenceEqual<T, TSource, TSourceEnumerator, TSecond, TSecondEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ref NativeArray<bool> output,
            in ValueSequence<T, TSecond, TSecondEnumerator> second
            )
            where T : struct, IEquatable<T>
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TSecond : struct, ISequence<T, TSecondEnumerator>
            where TSecondEnumerator : struct, IEnumerator<T>
        {
            var func = source.SequenceEqualAsFunc(second);
            return source.Schedule(func, ref output);
        }

        public static bool SequenceEqual<T, TSource, TSourceEnumerator, TSecond, TSecondEnumerator, TComparer>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            in ValueSequence<T, TSecond, TSecondEnumerator> second,
            in TComparer comparer
            )
            where T : struct, IEquatable<T>
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TSecond : struct, ISequence<T, TSecondEnumerator>
            where TSecondEnumerator : struct, IEnumerator<T>
            where TComparer : struct, IEqualityComparer<T>
        {
            var sourceList = source.ToNativeList(Allocator.Temp);
            var secondList = second.ToNativeList(Allocator.Temp);

            if (sourceList.Length != secondList.Length)
            {
                sourceList.Dispose();
                secondList.Dispose();
                return false;
            }
            for (var i = 0; i < sourceList.Length; i++)
            {
                if (!comparer.Equals(sourceList[i], secondList[i]))
                {
                    sourceList.Dispose();
                    secondList.Dispose();
                    return false;
                }
            }
            sourceList.Dispose();
            secondList.Dispose();
            return true;
        }

        public struct SequenceSequenceEqualFunc<T, TSource, TSourceEnumerator, TSecond, TSecondEnumerator, TComparer>
            : IFunc<ValueSequence<T, TSource, TSourceEnumerator>, bool>
            where T : struct, IEquatable<T>
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TSecond : struct, ISequence<T, TSecondEnumerator>
            where TSecondEnumerator : struct, IEnumerator<T>
            where TComparer : struct, IEqualityComparer<T>
        {
            public ValueSequence<T, TSecond, TSecondEnumerator> Second;
            public TComparer Comparer;

            public bool Invoke(ValueSequence<T, TSource, TSourceEnumerator> source)
            {
                return source.SequenceEqual(Second, Comparer);
            }
        }

        public static ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, bool>.Struct<SequenceSequenceEqualFunc<T, TSource, TSourceEnumerator, TSecond, TSecondEnumerator, TComparer>>
        SequenceEqualAsFunc<T, TSource, TSourceEnumerator, TSecond, TSecondEnumerator, TComparer>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            in ValueSequence<T, TSecond, TSecondEnumerator> second,
            in TComparer comparer
            )
            where T : struct, IEquatable<T>
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TSecond : struct, ISequence<T, TSecondEnumerator>
            where TSecondEnumerator : struct, IEnumerator<T>
            where TComparer : struct, IEqualityComparer<T>
        {
            var func = new SequenceSequenceEqualFunc<T, TSource, TSourceEnumerator, TSecond, TSecondEnumerator, TComparer> { Second = second, Comparer = comparer };
            return ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, bool>.New(func);
        }

        public static bool RunSequenceEqual<T, TSource, TSourceEnumerator, TSecond, TSecondEnumerator, TComparer>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            in ValueSequence<T, TSecond, TSecondEnumerator> second,
            in TComparer comparer
            )
            where T : struct, IEquatable<T>
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TSecond : struct, ISequence<T, TSecondEnumerator>
            where TSecondEnumerator : struct, IEnumerator<T>
            where TComparer : struct, IEqualityComparer<T>
        {
            var func = source.SequenceEqualAsFunc(second, comparer);
            return source.Run(func);
        }

        public static JobHandle<bool> ScheduleSequenceEqual<T, TSource, TSourceEnumerator, TSecond, TSecondEnumerator, TComparer>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            in ValueSequence<T, TSecond, TSecondEnumerator> second,
            in TComparer comparer
            )
            where T : struct, IEquatable<T>
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TSecond : struct, ISequence<T, TSecondEnumerator>
            where TSecondEnumerator : struct, IEnumerator<T>
            where TComparer : struct, IEqualityComparer<T>
        {
            var func = source.SequenceEqualAsFunc(second, comparer);
            return source.Schedule(func);
        }

        public static JobHandle ScheduleSequenceEqual<T, TSource, TSourceEnumerator, TSecond, TSecondEnumerator, TComparer>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ref NativeArray<bool> output,
            in ValueSequence<T, TSecond, TSecondEnumerator> second,
            in TComparer comparer
            )
            where T : struct, IEquatable<T>
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TSecond : struct, ISequence<T, TSecondEnumerator>
            where TSecondEnumerator : struct, IEnumerator<T>
            where TComparer : struct, IEqualityComparer<T>
        {
            var func = source.SequenceEqualAsFunc(second, comparer);
            return source.Schedule(func, ref output);
        }

        public static bool SequenceEqual<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            in NativeArray<T> second
            )
            where T : struct, IEquatable<T>
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var sourceList = source.ToNativeList(Allocator.Temp);
            var result = sourceList.AsArray().ArraysEqual(second);
            sourceList.Dispose();
            return result;
        }

        public struct SequenceSequenceEqualFunc<T, TSource, TSourceEnumerator>
            : IFunc<ValueSequence<T, TSource, TSourceEnumerator>, bool>
            where T : struct, IEquatable<T>
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            public NativeArray<T> Second;

            public bool Invoke(ValueSequence<T, TSource, TSourceEnumerator> source)
            {
                return source.SequenceEqual(Second);
            }
        }

        public static ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, bool>.Struct<SequenceSequenceEqualFunc<T, TSource, TSourceEnumerator>>
        SequenceEqualAsFunc<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            in NativeArray<T> second
            )
            where T : struct, IEquatable<T>
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var func = new SequenceSequenceEqualFunc<T, TSource, TSourceEnumerator> { Second = second };
            return ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, bool>.New(func);
        }

        public static bool RunSequenceEqual<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            in NativeArray<T> second
            )
            where T : struct, IEquatable<T>
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var func = source.SequenceEqualAsFunc(second);
            return source.Run(func);
        }

        public static JobHandle<bool> ScheduleSequenceEqual<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            in NativeArray<T> second
            )
            where T : struct, IEquatable<T>
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var func = source.SequenceEqualAsFunc(second);
            return source.Schedule(func);
        }

        public static JobHandle ScheduleSequenceEqual<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ref NativeArray<bool> output,
            in NativeArray<T> second
            )
            where T : struct, IEquatable<T>
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var func = source.SequenceEqualAsFunc(second);
            return source.Schedule(func, ref output);
        }

        public static bool SequenceEqual<T, TSource, TSourceEnumerator, TComparer>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            in NativeArray<T> second,
            in TComparer comparer
            )
            where T : struct, IEquatable<T>
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TComparer : struct, IEqualityComparer<T>
        {
            var sourceList = source.ToNativeList(Allocator.Temp);

            if (sourceList.Length != second.Length)
            {
                sourceList.Dispose();
                return false;
            }
            for (var i = 0; i < sourceList.Length; i++)
                if (!comparer.Equals(sourceList[i], second[i]))
                {
                    sourceList.Dispose();
                    return false;
                }
            sourceList.Dispose();
            return true;
        }

        public struct SequenceSequenceEqualFunc<T, TSource, TSourceEnumerator, TComparer>
            : IFunc<ValueSequence<T, TSource, TSourceEnumerator>, bool>
            where T : struct, IEquatable<T>
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TComparer : struct, IEqualityComparer<T>
        {
            public NativeArray<T> Second;
            public TComparer Comparer;

            public bool Invoke(ValueSequence<T, TSource, TSourceEnumerator> source)
            {
                return source.SequenceEqual(Second, Comparer);
            }
        }

        public static ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, bool>.Struct<SequenceSequenceEqualFunc<T, TSource, TSourceEnumerator, TComparer>>
        SequenceEqualAsFunc<T, TSource, TSourceEnumerator, TComparer>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            in NativeArray<T> second,
            in TComparer comparer
            )
            where T : struct, IEquatable<T>
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TComparer : struct, IEqualityComparer<T>
        {
            var func = new SequenceSequenceEqualFunc<T, TSource, TSourceEnumerator, TComparer> { Second = second, Comparer = comparer };
            return ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, bool>.New(func);
        }

        public static bool RunSequenceEqual<T, TSource, TSourceEnumerator, TComparer>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            in NativeArray<T> second,
            in TComparer comparer
            )
            where T : struct, IEquatable<T>
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TComparer : struct, IEqualityComparer<T>
        {
            var func = source.SequenceEqualAsFunc(second, comparer);
            return source.Run(func);
        }

        public static JobHandle<bool> ScheduleSequenceEqual<T, TSource, TSourceEnumerator, TComparer>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            in NativeArray<T> second,
            in TComparer comparer
            )
            where T : struct, IEquatable<T>
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TComparer : struct, IEqualityComparer<T>
        {
            var func = source.SequenceEqualAsFunc(second, comparer);
            return source.Schedule(func);
        }

        public static JobHandle ScheduleSequenceEqual<T, TSource, TSourceEnumerator, TComparer>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ref NativeArray<bool> output,
            in NativeArray<T> second,
            in TComparer comparer
            )
            where T : struct, IEquatable<T>
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TComparer : struct, IEqualityComparer<T>
        {
            var func = source.SequenceEqualAsFunc(second, comparer);
            return source.Schedule(func, ref output);
        }
    }
}
