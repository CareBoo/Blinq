using System;
using System.Collections.Generic;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static bool SequenceEqual<T, TSecond>(
            this ref NativeArray<T> source,
            ValueSequence<T, TSecond> second
            )
            where T : struct, IEquatable<T>
            where TSecond : struct, ISequence<T>
        {
            return source.ToValueSequence().SequenceEqual(second);
        }

        public static bool SequenceEqual<T, TSecond, TComparer>(
            this ref NativeArray<T> source,
            ValueSequence<T, TSecond> second,
            TComparer comparer
            )
            where T : struct, IEquatable<T>
            where TSecond : struct, ISequence<T>
            where TComparer : struct, IEqualityComparer<T>
        {
            return source.ToValueSequence().SequenceEqual(second, comparer);
        }

        public static bool SequenceEqual<T>(
            this ref NativeArray<T> source,
            NativeArray<T> second
            )
            where T : struct, IEquatable<T>
        {
            return source.ArraysEqual(second);
        }

        public static bool SequenceEqual<T, TComparer>(
            this ref NativeArray<T> source,
            NativeArray<T> second,
            TComparer comparer
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
