using System;
using Unity.Burst;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public partial struct ValueSequence<T, TSource>
        where T : struct, IEquatable<T>
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
