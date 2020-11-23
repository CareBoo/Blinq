using Unity.Jobs;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public struct CollectionJobHandle<TResult>
            where TResult : struct
        {
            JobHandle jobHandle;
            TResult output;

            public bool IsCompleted => jobHandle.IsCompleted;

            internal CollectionJobHandle(in JobHandle jobHandle, in TResult output)
            {
                this.jobHandle = jobHandle;
                this.output = output;
            }

            public TResult Complete()
            {
                jobHandle.Complete();
                return output;
            }
        }
    }
}
