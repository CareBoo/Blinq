using System.Collections.Generic;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public interface IOrderedSequence<T>
        : ISequence<T>
        , IComparer<T>
        where T : struct
    {
        NativeList<T> ExecuteUnordered();
    }
}
