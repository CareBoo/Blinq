using System;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, IntersectSequence<T, TSource, TSecond>> Intersect<T, TSource, TSecond>(
            this ValueSequence<T, TSource> source,
            ValueSequence<T, TSecond> second
            )
            where T : unmanaged, IEquatable<T>
            where TSource : struct, ISequence<T>
            where TSecond : struct, ISequence<T>
        {
            var seq = new IntersectSequence<T, TSource, TSecond> { Source = source.Source, Second = second.Source };
            return ValueSequence<T>.New(seq);
        }

        public static ValueSequence<T, IntersectSequence<T, TSource, NativeArraySequence<T>>> Intersect<T, TSource>(
            this ValueSequence<T, TSource> source,
            NativeArray<T> second
            )
            where T : unmanaged, IEquatable<T>
            where TSource : struct, ISequence<T>
        {
            return source.Intersect(second.ToValueSequence());
        }
    }

    public struct IntersectSequence<T, TSource, TSecond> : ISequence<T>
        where T : unmanaged, IEquatable<T>
        where TSource : struct, ISequence<T>
        where TSecond : struct, ISequence<T>
    {
        public TSource Source;
        public TSecond Second;

        public NativeList<T> ToList()
        {
            using (var sourceList = Source.ToList())
            using (var secondList = Second.ToList())
            using (var sourceSet = new NativeHashSet<T>(sourceList.Length, Allocator.Temp))
            using (var secondSet = new NativeHashSet<T>(secondList.Length, Allocator.Temp))
            {
                var result = new NativeList<T>(secondList.Length, Allocator.Temp);
                for (var i = 0; i < sourceList.Length; i++)
                    sourceSet.Add(sourceList[i]);
                for (var i = 0; i < secondList.Length; i++)
                    if (sourceSet.Contains(secondList[i]) && secondSet.Add(secondList[i]))
                        result.AddNoResize(secondList[i]);
                return result;
            }
        }
    }
}
