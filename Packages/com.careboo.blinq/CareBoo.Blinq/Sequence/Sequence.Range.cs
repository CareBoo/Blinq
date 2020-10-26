using System.Collections;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<int, RangeSequence> Range(int start, int count)
        {
            var seq = new RangeSequence { Start = start, Count = count };
            return ValueSequence<int>.New(seq);
        }
    }

    public struct RangeSequence : ISequence<int>
    {
        public int Start;
        public int Count;

        int currentIndex;

        public int Current => Start + currentIndex - 1;

        object IEnumerator.Current => Current;

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            currentIndex += 1;
            return currentIndex <= Count;
        }

        public void Reset()
        {
            currentIndex = 0;
        }

        public NativeList<int> ToList()
        {
            var list = new NativeList<int>(Count, Allocator.Temp);
            for (var i = 0; i < Count; i++)
                list.AddNoResize(i + Start);
            return list;
        }
    }
}
