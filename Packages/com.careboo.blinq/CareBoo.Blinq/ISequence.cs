using System.Collections.Generic;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public interface INativeListConvertible<T>
        where T : struct
    {
        NativeList<T> ToNativeList(Allocator allocator);
    }

    public interface ISequence<T, TEnumerator>
        : IEnumerable<T>
        , INativeListConvertible<T>
        where T : struct
        where TEnumerator : struct, IEnumerator<T>
    {
        new TEnumerator GetEnumerator();
    }
}
