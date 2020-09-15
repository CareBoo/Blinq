using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static NativeArray<T> ToNativeArray<T, TSource>(
            this ValueSequence<T, TSource> source,
            Allocator allocator
            )
            where T : struct
            where TSource : struct, ISequence<T>
        {
            var list = source.Execute();
            var result = new NativeArray<T>(list, allocator);
            list.Dispose();
            return result;
        }
    }
}
