using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, PrependSequence<T, NativeArraySequence<T>>> Prepend<T>(
            this ref NativeArray<T> source,
            T item
            )
            where T : struct
        {
            return source.ToValueSequence().Prepend(item);
        }
    }
}
