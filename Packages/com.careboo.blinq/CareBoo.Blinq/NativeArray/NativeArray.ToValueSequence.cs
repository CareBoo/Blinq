using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class NativeArrayExtensions
    {
        public struct NativeArraySourceSequence<T> : ISequence<T>
            where T : struct
        {
            public NativeArray<T> Source;

            public NativeList<T> Execute()
            {
                var list = new NativeList<T>(Source.Length, Allocator.Temp);
                for (var i = 0; i < Source.Length; i++)
                    list.AddNoResize(Source[i]);
                return list;
            }
        }

        public static ValueSequence<T, NativeArraySourceSequence<T>> ToValueSequence<T>(this ref NativeArray<T> nativeArray)
            where T : struct
        {
            var newSequence = new NativeArraySourceSequence<T> { Source = nativeArray };
            return new ValueSequence<T, NativeArraySourceSequence<T>>(newSequence);
        }
    }
}
