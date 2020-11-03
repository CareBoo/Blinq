using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, TakeSequence<T, NativeArraySequence<T>>> Take<T>(
            this in NativeArray<T> source,
            in int count
            )
            where T : struct
        {
            var sourceSeq = source.ToValueSequence();
            return sourceSeq.Take(in count);
        }
    }
}
