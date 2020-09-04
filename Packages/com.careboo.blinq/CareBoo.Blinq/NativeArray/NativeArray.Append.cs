using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class NativeArrayExtensions
    {
        public static ValueSequence<T, ValueSequence<T, NativeArraySourceQuery<T>>.AppendQuery> Append<T>(this ref NativeArray<T> source, T item)
            where T : struct
        {
            return source.ToValueSequence().Append(item);
        }
    }
}
