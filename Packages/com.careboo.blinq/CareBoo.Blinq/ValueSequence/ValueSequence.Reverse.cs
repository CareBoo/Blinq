using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, ReverseSequence<T, TSource>, SequenceEnumerator<T, ReverseSequence<T, TSource>>> Reverse<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var sourceSeq = source.Source;
            var seq = new ReverseSequence<T, TSource>(in sourceSeq);
            return ValueSequence<T, SequenceEnumerator<T, ReverseSequence<T, TSource>>>.New(in seq);
        }
    }

    public struct ReverseSequence<T, TSource>
        : ISequence<T, SequenceEnumerator<T, ReverseSequence<T, TSource>>>
        where T : struct
        where TSource : struct, INativeListConvertible<T>
    {
        readonly TSource source;

        public ReverseSequence(in TSource source)
        {
            this.source = source;
        }

        public SequenceEnumerator<T, ReverseSequence<T, TSource>> GetEnumerator()
        {
            return SequenceEnumerator<T>.New(in this);
        }

        public NativeList<T> ToNativeList(Allocator allocator)
        {
            var list = source.ToNativeList(allocator);
            for (var i = 0; i < list.Length / 2; i++)
            {
                var swap = list.Length - 1 - i;
                var tmp = list[i];
                list[i] = list[swap];
                list[swap] = tmp;
            }
            return list;
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
