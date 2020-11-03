using System;
using System.Collections.Generic;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static bool SequenceEqual<T, TSecond>(
            this in NativeArray<T> source,
            in ValueSequence<T, TSecond> second
            )
            where T : struct, IEquatable<T>
            where TSecond : struct, ISequence<T>
        {
            return source.ToValueSequence().SequenceEqual(in second);
        }

        public static bool SequenceEqual<T, TSecond, TComparer>(
            this in NativeArray<T> source,
            in ValueSequence<T, TSecond> second,
            in TComparer comparer
            )
            where T : struct, IEquatable<T>
            where TSecond : struct, ISequence<T>
            where TComparer : struct, IEqualityComparer<T>
        {
            return source.ToValueSequence().SequenceEqual(in second, in comparer);
        }

        public static bool SequenceEqual<T>(
            this in NativeArray<T> source,
            in NativeArray<T> second
            )
            where T : struct, IEquatable<T>
        {
            return source.ArraysEqual(second);
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
    }
}
