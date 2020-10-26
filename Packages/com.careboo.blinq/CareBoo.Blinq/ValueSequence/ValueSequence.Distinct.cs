using System;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, DistinctSequence<T, TSource>> Distinct<T, TSource>(
            this ValueSequence<T, TSource> source
            )
            where T : unmanaged, IEquatable<T>
            where TSource : struct, ISequence<T>
        {
            var seq = new DistinctSequence<T, TSource> { Source = source.Source };
            return ValueSequence<T>.New(seq);
        }
    }

    public struct DistinctSequence<T, TSource> : ISequence<T>
        where T : unmanaged, IEquatable<T>
        where TSource : struct, ISequence<T>
    {
        public TSource Source;

        public NativeList<T> ToList()
        {
            var list = Source.ToList();
            var set = new NativeHashSet<T>(list.Length, Allocator.Temp);
            for (var i = 0; i < list.Length; i++)
                if (!set.Add(list[i]))
                {
                    list.RemoveAt(i);
                    i--;
                }
            set.Dispose();
            return list;
        }
    }
}
