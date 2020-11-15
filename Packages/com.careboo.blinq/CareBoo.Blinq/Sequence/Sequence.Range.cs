using System;
using System.Collections;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<int, RangeSequence> Range(int start, int count)
        {
            var seq = new RangeSequence(in start, in count);
            return ValueSequence<int>.New(ref seq);
        }
    }

    public struct RangeSequence : ISequence<int>
    {
        readonly int start;
        readonly int count;

        int currentIndex;

        public RangeSequence(in int start, in int count)
        {
            this.start = start;
            this.count = count;
            currentIndex = -1;
        }

        public int Current => start + currentIndex;

        object IEnumerator.Current => Current;

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            currentIndex += 1;
            return currentIndex < count;
        }

        public void Reset()
        {
            throw new NotSupportedException();
        }

        public NativeList<int> ToNativeList(Allocator allocator)
        {
            var list = new NativeList<int>(count, allocator);
            for (var i = 0; i < count; i++)
                list.AddNoResize(i + start);
            return list;
        }
    }
}
