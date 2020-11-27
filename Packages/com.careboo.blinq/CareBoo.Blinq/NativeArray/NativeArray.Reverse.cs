using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, ReverseSequence<T, NativeArraySequence<T>>, SequenceEnumerator<T, ReverseSequence<T, NativeArraySequence<T>>>> Reverse<T>(
            this in NativeArray<T> source
            )
            where T : struct
        {
            return source.ToValueSequence().Reverse();
        }
    }
}
