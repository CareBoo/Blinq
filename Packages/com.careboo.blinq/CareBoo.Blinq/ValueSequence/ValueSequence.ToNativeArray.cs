using Unity.Burst;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public partial struct ValueSequence<T, TSource>
    {
        [BurstCompile]
        public NativeArray<T> ToNativeArray(Allocator allocator)
        {
            var list = Execute();
            var output = new NativeArray<T>(list, allocator);
            return output;
        }
    }
}
