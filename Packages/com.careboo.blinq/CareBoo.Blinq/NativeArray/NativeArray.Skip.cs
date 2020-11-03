using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, SkipSequence<T, NativeArraySequence<T>>> Skip<T>(
            this in NativeArray<T> source,
            in int count
            )
            where T : struct
        {
            var sourceSeq = source.ToValueSequence();
            return sourceSeq.Skip(in count);
        }
    }
}
