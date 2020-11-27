using System;
using System.Collections.Generic;
using Unity.Collections;

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

    }
}
