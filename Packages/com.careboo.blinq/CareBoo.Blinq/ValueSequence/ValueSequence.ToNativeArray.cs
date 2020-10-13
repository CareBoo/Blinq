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

        public static NativeArray<T> ToNativeArray<T, TSource>(
            this ValueSequence<T, TSource> source,
            NativeArray<T> output
            )
            where T : struct
            where TSource : struct, ISequence<T>
        {
            var list = source.Execute();
            for (var i = 0; i < list.Length; i++)
                output[i] = list[i];
            return output;
        }
    }
}
