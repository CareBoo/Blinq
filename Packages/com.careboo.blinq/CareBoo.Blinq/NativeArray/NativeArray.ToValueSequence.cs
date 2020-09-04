using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class NativeArrayExtensions
    {
        public struct NativeArraySourceQuery<T> : IQuery<T>
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

        public static ValueSequence<T, NativeArraySourceQuery<T>> ToValueSequence<T>(this ref NativeArray<T> nativeArray)
            where T : struct
        {
            var newQuery = new NativeArraySourceQuery<T> { Source = nativeArray };
            return new ValueSequence<T, NativeArraySourceQuery<T>>(newQuery);
        }
    }
}
