using Unity.Collections;

namespace CareBoo.Blinq
{
    public interface IQuery<T>
        where T : struct
    {
        NativeList<T> Execute();
    }
}
