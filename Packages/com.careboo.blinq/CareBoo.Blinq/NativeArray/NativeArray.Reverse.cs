using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, ReverseSequence<T, NativeArraySequence<T>>> Reverse<T>(
            this ref NativeArray<T> source
            )
            where T : struct
        {
            var sourceSeq = source.ToValueSequence();
            return sourceSeq.Reverse();
        }
    }
}
