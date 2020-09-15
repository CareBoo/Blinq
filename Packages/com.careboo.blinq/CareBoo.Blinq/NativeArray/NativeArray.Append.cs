using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, AppendSequence<T, NativeArraySequence<T>>> Append<T>(this ref NativeArray<T> source, T item)
            where T : struct
        {
            return source.ToValueSequence().Append(item);
        }
    }
}
