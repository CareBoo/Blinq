using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, RepeatSequence<T>> Repeat<T>(T element, int count)
            where T : struct
        {
            var seq = new RepeatSequence<T> { Element = element, Count = count };
            return ValueSequence<T>.New(seq);
        }
    }

    public struct RepeatSequence<T> : ISequence<T>
        where T : struct
    {
        public T Element;
        public int Count;

        public NativeList<T> Execute()
        {
            var list = new NativeList<T>(Count, Allocator.Temp);
            for (var i = 0; i < Count; i++)
                list.AddNoResize(Element);
            return list;
        }
    }
}
