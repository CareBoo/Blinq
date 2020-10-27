using System.Collections;
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

        private int currentCount;

        public T Current => Element;

        object IEnumerator.Current => Current;

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            currentCount -= 1;
            return currentCount >= 0;
        }

        public void Reset()
        {
            currentCount = Count;
        }

        public NativeList<T> ToList()
        {
            var list = new NativeList<T>(Count, Allocator.Temp);
            for (var i = 0; i < Count; i++)
                list.AddNoResize(Element);
            return list;
        }
    }
}
