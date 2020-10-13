using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, EmptySequence<T>> Empty<T>()
            where T : struct
        {
            var seq = new EmptySequence<T>();
            return ValueSequence<T>.New(seq);
        }
    }

    public struct EmptySequence<T> : ISequence<T>
        where T : struct
    {
        public NativeList<T> Execute()
        {
            return new NativeList<T>(Allocator.Temp);
        }
    }
}
