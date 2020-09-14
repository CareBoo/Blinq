using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class ValueSequenceExtensions
    {
        public static ValueSequence<T, ConcatSequence<T, TSource, TSecond>> Concat<T, TSource, TSecond>(
            this ref ValueSequence<T, TSource> source,
            TSecond second
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TSecond : struct, ISequence<T>
        {
            var newSequence = new ConcatSequence<T, TSource, TSecond> { Source = source.Source, Second = second };
            return new ValueSequence<T, ConcatSequence<T, TSource, TSecond>>(newSequence);
        }
    }

    public struct ConcatSequence<T, TSource, TSecond> : ISequence<T>
        where T : struct
        where TSource : struct, ISequence<T>
        where TSecond : struct, ISequence<T>
    {
        public TSource Source;
        public TSecond Second;

        public NativeList<T> Execute()
        {
            var first = Source.Execute();
            var second = Second.Execute();
            first.AddRange(second);
            second.Dispose();
            return first;
        }
    }
}
