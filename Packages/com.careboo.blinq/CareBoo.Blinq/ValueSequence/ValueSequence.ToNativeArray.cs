using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public partial struct ValueSequence<T, TQuery>
        : IEnumerable<T>
        where T : struct
        where TQuery : struct, IQuery<T>
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
