using System.Collections;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, ConcatSequence<T, TSource, TSecond>> Concat<T, TSource, TSecond>(
            this ValueSequence<T, TSource> source,
            TSecond second
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TSecond : struct, ISequence<T>
        {
            var newSequence = new ConcatSequence<T, TSource, TSecond> { Source = source.Source, Second = second };
            return ValueSequence<T>.New(newSequence);
        }

        public static ValueSequence<T, ConcatSequence<T, TSource, NativeArraySequence<T>>> Concat<T, TSource>(
            this ValueSequence<T, TSource> source,
            NativeArray<T> second
            )
            where T : struct
            where TSource : struct, ISequence<T>
        {
            var newSequence = new ConcatSequence<T, TSource, NativeArraySequence<T>> { Source = source.Source, Second = second.ToValueSequence().Source };
            return ValueSequence<T>.New(newSequence);
        }
    }

    public struct ConcatSequence<T, TSource, TSecond> : ISequence<T>
        where T : struct
        where TSource : struct, ISequence<T>
        where TSecond : struct, ISequence<T>
    {
        public TSource Source;
        public TSecond Second;

        bool currentIndex;

        public T Current => currentIndex
            ? Second.Current
            : Source.Current;

        object IEnumerator.Current => Current;

        public void Dispose()
        {

            Source.Dispose();
            Second.Dispose();
        }

        public bool MoveNext()
        {
            if (!Source.MoveNext())
            {
                currentIndex = true;
                return Second.MoveNext();
            }
            return true;
        }

        public void Reset()
        {
            currentIndex = false;
        }

        public NativeList<T> ToList()
        {
            var first = Source.ToList();
            var second = Second.ToList();
            first.AddRange(second);
            second.Dispose();
            return first;
        }
    }
}
