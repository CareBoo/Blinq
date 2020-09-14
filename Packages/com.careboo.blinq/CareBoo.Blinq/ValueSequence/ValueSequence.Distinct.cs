using System;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class ValueSequenceExtensions
    {
        public static ValueSequence<T, DistinctSequence<T, TSource>> Distinct<T, TSource>(
            this ref ValueSequence<T, TSource> source
            )
            where T : unmanaged, IEquatable<T>
            where TSource : struct, ISequence<T>
        {
            var seq = new DistinctSequence<T, TSource> { Source = source.Source };
            return new ValueSequence<T, DistinctSequence<T, TSource>>(seq);
        }
    }

    public struct DistinctSequence<T, TSource> : ISequence<T>
        where T : unmanaged, IEquatable<T>
        where TSource : struct, ISequence<T>
    {
        public TSource Source;

        public NativeList<T> Execute()
        {
            var list = Source.Execute();
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
