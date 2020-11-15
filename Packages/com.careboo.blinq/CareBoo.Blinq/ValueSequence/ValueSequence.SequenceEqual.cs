using System;
using System.Collections.Generic;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static bool SequenceEqual<T, TSource, TSecond>(
            this in ValueSequence<T, TSource> source,
            in ValueSequence<T, TSecond> second
            )
            where T : struct, IEquatable<T>
            where TSource : struct, ISequence<T>
            where TSecond : struct, ISequence<T>
        {
            var sourceList = source.ToNativeList(Allocator.Temp);
            var secondList = second.ToNativeList(Allocator.Temp);
            var result = sourceList.AsArray().ArraysEqual(secondList);
            sourceList.Dispose();
            secondList.Dispose();
            return result;
        }

        public static bool SequenceEqual<T, TSource, TSecond, TComparer>(
            this in ValueSequence<T, TSource> source,
            in ValueSequence<T, TSecond> second,
            in TComparer comparer
            )
            where T : struct, IEquatable<T>
            where TSource : struct, ISequence<T>
            where TSecond : struct, ISequence<T>
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

        public static bool SequenceEqual<T, TSource>(
            this in ValueSequence<T, TSource> source,
            in NativeArray<T> second
            )
            where T : struct, IEquatable<T>
            where TSource : struct, ISequence<T>
        {
            var sourceList = source.ToNativeList(Allocator.Temp);
            var result = sourceList.AsArray().ArraysEqual(second);
            sourceList.Dispose();
            return result;
        }

        public static bool SequenceEqual<T, TSource, TComparer>(
            this in ValueSequence<T, TSource> source,
            in NativeArray<T> second,
            in TComparer comparer
            )
            where T : struct, IEquatable<T>
            where TSource : struct, ISequence<T>
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

    }
}
