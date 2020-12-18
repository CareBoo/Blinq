using System;
using System.Collections.Generic;
using CareBoo.Burst.Delegates;
using Unity.Collections;
using Unity.Jobs;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static bool SequenceEqual<T, TSecond, TSecondEnumerator>(
            this in NativeArray<T> source,
            in ValueSequence<T, TSecond, TSecondEnumerator> second
            )
            where T : struct, IEquatable<T>
            where TSecond : struct, ISequence<T, TSecondEnumerator>
            where TSecondEnumerator : struct, IEnumerator<T>
        {
            return source.ToValueSequence().SequenceEqual(in second);
        }

        public struct ArraySequenceEqualFunc<T, TSecond, TSecondEnumerator>
            : IFunc<NativeArray<T>, bool>
            where T : struct, IEquatable<T>
            where TSecond : struct, ISequence<T, TSecondEnumerator>
            where TSecondEnumerator : struct, IEnumerator<T>
        {
            public ValueSequence<T, TSecond, TSecondEnumerator> Second;

            public bool Invoke(NativeArray<T> source)
            {
                return source.SequenceEqual(Second);
            }
        }

        public static ValueFunc<NativeArray<T>, bool>.Struct<ArraySequenceEqualFunc<T, TSecond, TSecondEnumerator>>
        SequenceEqualAsFunc<T, TSecond, TSecondEnumerator>(
            this in NativeArray<T> source,
            in ValueSequence<T, TSecond, TSecondEnumerator> second
            )
            where T : struct, IEquatable<T>
            where TSecond : struct, ISequence<T, TSecondEnumerator>
            where TSecondEnumerator : struct, IEnumerator<T>
        {
            var func = new ArraySequenceEqualFunc<T, TSecond, TSecondEnumerator> { Second = second };
            return ValueFunc<NativeArray<T>, bool>.New(func);
        }

        public static bool RunSequenceEqual<T, TSecond, TSecondEnumerator>(
            this in NativeArray<T> source,
            in ValueSequence<T, TSecond, TSecondEnumerator> second
            )
            where T : struct, IEquatable<T>
            where TSecond : struct, ISequence<T, TSecondEnumerator>
            where TSecondEnumerator : struct, IEnumerator<T>
        {
            var func = source.SequenceEqualAsFunc(second);
            return source.Run(func);
        }

        public static JobHandle<bool> ScheduleSequenceEqual<T, TSecond, TSecondEnumerator>(
            this in NativeArray<T> source,
            in ValueSequence<T, TSecond, TSecondEnumerator> second
            )
            where T : struct, IEquatable<T>
            where TSecond : struct, ISequence<T, TSecondEnumerator>
            where TSecondEnumerator : struct, IEnumerator<T>
        {
            var func = source.SequenceEqualAsFunc(second);
            return source.Schedule(func);
        }

        public static JobHandle ScheduleSequenceEqual<T, TSecond, TSecondEnumerator>(
            this in NativeArray<T> source,
            ref NativeArray<bool> output,
            in ValueSequence<T, TSecond, TSecondEnumerator> second
            )
            where T : struct, IEquatable<T>
            where TSecond : struct, ISequence<T, TSecondEnumerator>
            where TSecondEnumerator : struct, IEnumerator<T>
        {
            var func = source.SequenceEqualAsFunc(second);
            return source.Schedule(func, ref output);
        }


        public static bool SequenceEqual<T, TSecond, TSecondEnumerator, TComparer>(
            this in NativeArray<T> source,
            in ValueSequence<T, TSecond, TSecondEnumerator> second,
            in TComparer comparer
            )
            where T : struct, IEquatable<T>
            where TSecond : struct, ISequence<T, TSecondEnumerator>
            where TSecondEnumerator : struct, IEnumerator<T>
            where TComparer : struct, IEqualityComparer<T>
        {
            return source.ToValueSequence().SequenceEqual(in second, in comparer);
        }

        public struct ArraySequenceEqualFunc<T, TSecond, TSecondEnumerator, TComparer>
            : IFunc<NativeArray<T>, bool>
            where T : struct, IEquatable<T>
            where TSecond : struct, ISequence<T, TSecondEnumerator>
            where TSecondEnumerator : struct, IEnumerator<T>
            where TComparer : struct, IEqualityComparer<T>
        {
            public ValueSequence<T, TSecond, TSecondEnumerator> Second;
            public TComparer Comparer;

            public bool Invoke(NativeArray<T> source)
            {
                return source.SequenceEqual(Second, Comparer);
            }
        }

        public static ValueFunc<NativeArray<T>, bool>.Struct<ArraySequenceEqualFunc<T, TSecond, TSecondEnumerator, TComparer>>
        SequenceEqualAsFunc<T, TSecond, TSecondEnumerator, TComparer>(
            this in NativeArray<T> source,
            in ValueSequence<T, TSecond, TSecondEnumerator> second,
            in TComparer comparer
            )
            where T : struct, IEquatable<T>
            where TSecond : struct, ISequence<T, TSecondEnumerator>
            where TSecondEnumerator : struct, IEnumerator<T>
            where TComparer : struct, IEqualityComparer<T>
        {
            var func = new ArraySequenceEqualFunc<T, TSecond, TSecondEnumerator, TComparer> { Second = second, Comparer = comparer };
            return ValueFunc<NativeArray<T>, bool>.New(func);
        }

        public static bool RunSequenceEqual<T, TSecond, TSecondEnumerator, TComparer>(
            this in NativeArray<T> source,
            in ValueSequence<T, TSecond, TSecondEnumerator> second,
            in TComparer comparer
            )
            where T : struct, IEquatable<T>
            where TSecond : struct, ISequence<T, TSecondEnumerator>
            where TSecondEnumerator : struct, IEnumerator<T>
            where TComparer : struct, IEqualityComparer<T>
        {
            var func = source.SequenceEqualAsFunc(second, comparer);
            return source.Run(func);
        }

        public static JobHandle<bool> ScheduleSequenceEqual<T, TSecond, TSecondEnumerator, TComparer>(
            this in NativeArray<T> source,
            in ValueSequence<T, TSecond, TSecondEnumerator> second,
            in TComparer comparer
            )
            where T : struct, IEquatable<T>
            where TSecond : struct, ISequence<T, TSecondEnumerator>
            where TSecondEnumerator : struct, IEnumerator<T>
            where TComparer : struct, IEqualityComparer<T>
        {
            var func = source.SequenceEqualAsFunc(second, comparer);
            return source.Schedule(func);
        }

        public static JobHandle ScheduleSequenceEqual<T, TSecond, TSecondEnumerator, TComparer>(
            this in NativeArray<T> source,
            ref NativeArray<bool> output,
            in ValueSequence<T, TSecond, TSecondEnumerator> second,
            in TComparer comparer
            )
            where T : struct, IEquatable<T>
            where TSecond : struct, ISequence<T, TSecondEnumerator>
            where TSecondEnumerator : struct, IEnumerator<T>
            where TComparer : struct, IEqualityComparer<T>
        {
            var func = source.SequenceEqualAsFunc(second, comparer);
            return source.Schedule(func, ref output);
        }


        public static bool SequenceEqual<T>(
            this in NativeArray<T> source,
            in NativeArray<T> second
            )
            where T : struct, IEquatable<T>
        {
            return source.ArraysEqual(second);
        }

        public struct ArraySequenceEqualFunc<T>
            : IFunc<NativeArray<T>, bool>
            where T : struct, IEquatable<T>
        {
            public NativeArray<T> Second;

            public bool Invoke(NativeArray<T> source)
            {
                return source.SequenceEqual(Second);
            }
        }

        public static ValueFunc<NativeArray<T>, bool>.Struct<ArraySequenceEqualFunc<T>>
        SequenceEqualAsFunc<T>(
            this in NativeArray<T> source,
            in NativeArray<T> second
            )
            where T : struct, IEquatable<T>
        {
            var func = new ArraySequenceEqualFunc<T> { Second = second };
            return ValueFunc<NativeArray<T>, bool>.New(func);
        }

        public static bool RunSequenceEqual<T>(
            this in NativeArray<T> source,
            in NativeArray<T> second
            )
            where T : struct, IEquatable<T>
        {
            var func = source.SequenceEqualAsFunc(second);
            return source.Run(func);
        }

        public static JobHandle<bool> ScheduleSequenceEqual<T>(
            this in NativeArray<T> source,
            in NativeArray<T> second
            )
            where T : struct, IEquatable<T>
        {
            var func = source.SequenceEqualAsFunc(second);
            return source.Schedule(func);
        }

        public static JobHandle ScheduleSequenceEqual<T>(
            this in NativeArray<T> source,
            ref NativeArray<bool> output,
            in NativeArray<T> second
            )
            where T : struct, IEquatable<T>
        {
            var func = source.SequenceEqualAsFunc(second);
            return source.Schedule(func, ref output);
        }


        public static bool SequenceEqual<T, TComparer>(
            this in NativeArray<T> source,
            in NativeArray<T> second,
            in TComparer comparer
            )
            where T : struct, IEquatable<T>
            where TComparer : struct, IEqualityComparer<T>
        {
            if (source.Length != second.Length)
                return false;
            for (var i = 0; i < source.Length; i++)
            {
                if (!comparer.Equals(source[i], second[i]))
                    return false;
            }
            return true;
        }

        public struct ArraySequenceEqualFunc<T, TComparer>
            : IFunc<NativeArray<T>, bool>
            where T : struct, IEquatable<T>
            where TComparer : struct, IEqualityComparer<T>
        {
            public NativeArray<T> Second;
            public TComparer Comparer;

            public bool Invoke(NativeArray<T> source)
            {
                return source.SequenceEqual(Second, Comparer);
            }
        }

        public static ValueFunc<NativeArray<T>, bool>.Struct<ArraySequenceEqualFunc<T, TComparer>>
        SequenceEqualAsFunc<T, TComparer>(
            this in NativeArray<T> source,
            in NativeArray<T> second,
            in TComparer comparer
            )
            where T : struct, IEquatable<T>
            where TComparer : struct, IEqualityComparer<T>
        {
            var func = new ArraySequenceEqualFunc<T, TComparer> { Second = second, Comparer = comparer };
            return ValueFunc<NativeArray<T>, bool>.New(func);
        }

        public static bool RunSequenceEqual<T, TComparer>(
            this in NativeArray<T> source,
            in NativeArray<T> second,
            in TComparer comparer
            )
            where T : struct, IEquatable<T>
            where TComparer : struct, IEqualityComparer<T>
        {
            var func = source.SequenceEqualAsFunc(second, comparer);
            return source.Run(func);
        }

        public static JobHandle<bool> ScheduleSequenceEqual<T, TComparer>(
            this in NativeArray<T> source,
            in NativeArray<T> second,
            in TComparer comparer
            )
            where T : struct, IEquatable<T>
            where TComparer : struct, IEqualityComparer<T>
        {
            var func = source.SequenceEqualAsFunc(second, comparer);
            return source.Schedule(func);
        }

        public static JobHandle ScheduleSequenceEqual<T, TComparer>(
            this in NativeArray<T> source,
            ref NativeArray<bool> output,
            in NativeArray<T> second,
            in TComparer comparer
            )
            where T : struct, IEquatable<T>
            where TComparer : struct, IEqualityComparer<T>
        {
            var func = source.SequenceEqualAsFunc(second, comparer);
            return source.Schedule(func, ref output);
        }
    }
}
