using System;
using System.Collections;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, ReverseSequence<T, TSource>> Reverse<T, TSource>(
            this ref ValueSequence<T, TSource> source
            )
            where T : struct
            where TSource : struct, ISequence<T>
        {
            var sourceSeq = source.GetEnumerator();
            var seq = new ReverseSequence<T, TSource>(ref sourceSeq);
            return ValueSequence<T>.New(ref seq);
        }
    }

    public struct ReverseSequence<T, TSource> : ISequence<T>
        where T : struct
        where TSource : struct, ISequence<T>
    {
        TSource source;

        int currentIndex;
        NativeList<T> sourceList;

        public ReverseSequence(ref TSource source)
        {
            this.source = source;
            currentIndex = 0;
            sourceList = default;
        }

        public T Current => sourceList[currentIndex];

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            source.Dispose();
            sourceList.Dispose();
        }

        public bool MoveNext()
        {
            if (!sourceList.IsCreated)
                sourceList = ToList();
            else
                currentIndex += 1;
            return currentIndex < sourceList.Length;
        }

        public void Reset()
        {
            throw new NotSupportedException();
        }

        public NativeList<T> ToList()
        {
            var list = source.ToList();
            for (var i = 0; i < list.Length / 2; i++)
            {
                var swap = list.Length - 1 - i;
                var tmp = list[i];
                list[i] = list[swap];
                list[swap] = tmp;
            }
            return list;
        }
    }
}
