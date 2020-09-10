using Unity.Burst;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public partial struct ValueSequence<T, TSource>
    {
        [BurstCompile]
        public NativeList<T> ToNativeList(Allocator allocator)
        {
            var output = new NativeList<T>(allocator);
            ToNativeList(output);
            return output;
        }

        [BurstCompile]
        public void ToNativeList(NativeList<T> output)
        {
            var list = Execute();
            output.CopyFrom(list);
            list.Dispose();
        }
    }
}
