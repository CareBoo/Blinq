using System;
using System.Collections.Generic;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static bool SequenceEqual<T, TSource, TSecond>(
            this ValueSequence<T, TSource> source,
            ValueSequence<T, TSecond> second
            )
            where T : struct, IEquatable<T>
            where TSource : struct, ISequence<T>
            where TSecond : struct, ISequence<T>
        {
            using (var sourceList = source.Execute())
            using (var secondList = second.Execute())
            {
                return sourceList.AsArray().ArraysEqual(secondList);
            }
        }

        public static bool SequenceEqual<T, TSource, TSecond, TComparer>(
            this ValueSequence<T, TSource> source,
            ValueSequence<T, TSecond> second,
            TComparer comparer
            )
            where T : struct, IEquatable<T>
            where TSource : struct, ISequence<T>
            where TSecond : struct, ISequence<T>
            where TComparer : struct, IEqualityComparer<T>
        {
            using (var sourceList = source.Execute())
            using (var secondList = second.Execute())
            {
                if (sourceList.Length != secondList.Length)
                    return false;
                for (var i = 0; i < sourceList.Length; i++)
                {
                    if (!comparer.Equals(sourceList[i], secondList[i]))
                        return false;
                }
                return true;
            }
        }

        public static bool SequenceEqual<T, TSource>(
            this ValueSequence<T, TSource> source,
            NativeArray<T> second
            )
            where T : struct, IEquatable<T>
            where TSource : struct, ISequence<T>
        {
            using (var sourceList = source.Execute())
            {
                return sourceList.AsArray().ArraysEqual(second);
            }
        }

        public static bool SequenceEqual<T, TSource, TComparer>(
            this ValueSequence<T, TSource> source,
            NativeArray<T> second,
            TComparer comparer
            )
            where T : struct, IEquatable<T>
            where TSource : struct, ISequence<T>
            where TComparer : struct, IEqualityComparer<T>
        {
            using (var sourceList = source.Execute())
            {
                if (sourceList.Length != second.Length)
                    return false;
                for (var i = 0; i < sourceList.Length; i++)
                {
                    if (!comparer.Equals(sourceList[i], second[i]))
                        return false;
                }
                return true;
            }
        }

    }
}
