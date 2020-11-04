using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static NativeList<T> ToNativeList<T, TSource>(
            this in ValueSequence<T, TSource> source,
            in Allocator allocator
            )
            where T : struct
            where TSource : struct, ISequence<T>
        {
            var list = source.Execute();
            var result = new NativeList<T>(allocator);
            result.CopyFrom(list);
            list.Dispose();
            return result;
        }
    }
}
