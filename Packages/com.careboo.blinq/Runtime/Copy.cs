using Unity.Collections;
using Unity.Jobs;

namespace CareBoo.Blinq
{
    public struct Copy<T> : IJobParallelFor
        where T : struct
    {
        [ReadOnly]
        public NativeArray<T> Input;

        [WriteOnly]
        public NativeArray<T> Output;

        public void Execute(int index)
        {
            Output[index] = Input[index];
        }
    }
}
