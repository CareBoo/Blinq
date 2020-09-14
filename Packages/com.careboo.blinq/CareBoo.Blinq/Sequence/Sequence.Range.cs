using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<int, RangeSequence> Range(int start, int count)
        {
            var seq = new RangeSequence { Start = start, Count = count };
            return new ValueSequence<int, RangeSequence>(seq);
        }
    }

    public struct RangeSequence : ISequence<int>
    {
        public int Start;
        public int Count;

        public NativeList<int> Execute()
        {
            var list = new NativeList<int>(Count, Allocator.Temp);
            for (var i = 0; i < Count; i++)
                list.AddNoResize(i + Start);
            return list;
        }
    }
}
