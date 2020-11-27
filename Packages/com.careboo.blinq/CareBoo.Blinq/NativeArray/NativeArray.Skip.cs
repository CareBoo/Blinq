using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, SkipSequence<T, NativeArraySequence<T>, NativeArray<T>.Enumerator>, SkipSequence<T, NativeArraySequence<T>, NativeArray<T>.Enumerator>.Enumerator> Skip<T>(
            this in NativeArray<T> source,
            int count
            )
            where T : struct
        {
            var sourceSeq = source.ToValueSequence();
            return sourceSeq.Skip(count);
        }
    }
}
