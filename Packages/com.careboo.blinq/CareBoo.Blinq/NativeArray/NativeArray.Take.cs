using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, TakeSequence<T, NativeArraySequence<T>>> Take<T>(
            this ref NativeArray<T> source,
            int count
            )
            where T : struct
        {
            return source.ToValueSequence().Take(count);
        }
    }
}
