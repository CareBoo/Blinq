using System.Collections.Generic;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public interface ISequence<T> : IEnumerator<T>
        where T : struct
    {
        NativeList<T> ToList();
    }
}
