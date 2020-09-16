using Unity.Collections;

namespace CareBoo.Blinq
{
    public interface ISequence<T>
        where T : struct
    {
        NativeList<T> Execute();
    }
}
