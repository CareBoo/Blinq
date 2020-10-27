using System.Collections;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public struct NativeArraySequence<T> : ISequence<T>
        where T : struct
    {
        public NativeArray<T> Source;

        private int currentIndex;

        public T Current => Source[currentIndex - 1];

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            Source.Dispose();
        }

        public bool MoveNext()
        {
            currentIndex += 1;
            return currentIndex <= Source.Length;
        }

        public void Reset()
        {
            currentIndex = 0;
        }

        public NativeList<T> ToList()
        {
            var list = new NativeList<T>(Source.Length, Allocator.Temp);
            for (var i = 0; i < Source.Length; i++)
                list.AddNoResize(Source[i]);
            return list;
        }
    }

    public static partial class Sequence
    {
        public static ValueSequence<T, NativeArraySequence<T>> ToValueSequence<T>(this ref NativeArray<T> nativeArray)
            where T : struct
        {
            var newSequence = new NativeArraySequence<T> { Source = nativeArray };
            return ValueSequence<T>.New(newSequence);
        }
    }
}
