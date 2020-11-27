using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, AppendSequence<T, NativeArraySequence<T>, NativeArray<T>.Enumerator>, AppendSequence<T, NativeArraySequence<T>, NativeArray<T>.Enumerator>.Enumerator> Append<T>(this in NativeArray<T> source, T item)
            where T : struct
        {
            var sourceSeq = source.ToValueSequence();
            return sourceSeq.Append(item);
        }
    }
}
