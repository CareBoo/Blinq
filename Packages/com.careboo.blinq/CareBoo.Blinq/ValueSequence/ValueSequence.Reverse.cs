using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, ReverseSequence<T, TSource>> Reverse<T, TSource>(
            this ValueSequence<T, TSource> source
            )
            where T : struct
            where TSource : struct, ISequence<T>
        {
            var seq = new ReverseSequence<T, TSource> { Source = source.Source };
            return new ValueSequence<T, ReverseSequence<T, TSource>>(seq);
        }
    }

    public struct ReverseSequence<T, TSource> : ISequence<T>
        where T : struct
        where TSource : struct, ISequence<T>
    {
        public TSource Source;

        public NativeList<T> Execute()
        {
            var list = Source.Execute();
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
