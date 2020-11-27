using System.Collections.Generic;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public interface IOrderedNativeListConvertible<T>
        : IComparer<T>
        where T : struct
    {
        NativeList<T> ToUnorderedList(Allocator allocator);
    }
    public interface IOrderedSequence<T, TEnumerator>
        : ISequence<T, TEnumerator>
        , IOrderedNativeListConvertible<T>
        where T : struct
        where TEnumerator : struct, IEnumerator<T>
    {
    }
}
