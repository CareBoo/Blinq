using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, SkipSequence<T, NativeArraySequence<T>>> Skip<T>(
            this ref NativeArray<T> source,
            int count
            )
            where T : struct
        {
            return source.ToValueSequence().Skip(count);
        }
    }
}
