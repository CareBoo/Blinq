using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<
            T,
            DefaultIfEmptySequence<T, NativeArraySequence<T>, NativeArray<T>.Enumerator>,
            DefaultIfEmptySequence<T, NativeArraySequence<T>, NativeArray<T>.Enumerator>.Enumerator>
        DefaultIfEmpty<T>(
            this in NativeArray<T> source,
            in T defaultVal = default
            )
            where T : struct
        {
            return source.ToValueSequence().DefaultIfEmpty(in defaultVal);
        }
    }
}
