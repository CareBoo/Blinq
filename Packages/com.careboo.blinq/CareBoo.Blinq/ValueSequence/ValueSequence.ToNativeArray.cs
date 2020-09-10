using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public partial struct ValueSequence<T, TSource>
        : IEnumerable<T>
        where T : struct
        where TSource : struct, ISequence<T>
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
