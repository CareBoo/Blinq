using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, TakeSequence<T, NativeArraySequence<T>, NativeArray<T>.Enumerator>, TakeSequence<T, NativeArraySequence<T>, NativeArray<T>.Enumerator>.Enumerator> Take<T>(
            this in NativeArray<T> source,
            int count
            )
            where T : struct
        {
            return source.ToValueSequence().Take(count);
        }
    }
}
