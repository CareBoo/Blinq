using System.Collections.Generic;

namespace CareBoo.Blinq
{
    public interface IOrderedSequence<T, TComparer>
        : ISequence<T>
        where T : struct
        where TComparer : struct, IComparer<T>
    {
        TComparer GetComparer();
    }
}
