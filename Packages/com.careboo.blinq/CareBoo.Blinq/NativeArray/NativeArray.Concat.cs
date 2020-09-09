using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class NativeArrayExtensions
    {
        public static ValueSequence<T, ValueSequence<T, NativeArraySourceQuery<T>>.ConcatQuery<TSequence>> Concat<T, TSequence>(
            this ref NativeArray<T> source,
            TSequence second
            )
            where T : struct
            where TSequence : struct, IQuery<T>
        {
            return source.ToValueSequence().Concat(second);
        }
    }
}
