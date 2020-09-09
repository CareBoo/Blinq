using System.Collections.Generic;
using Unity.Jobs;

namespace CareBoo.Blinq
{
    public struct SequenceExecutionJobHandle<T, TResult>
        where T : struct
        where TResult : struct, IEnumerable<T>
    {
        readonly JobHandle scheduledJob;
        readonly TResult output;

        public bool IsCompleted => scheduledJob.IsCompleted;

        public SequenceExecutionJobHandle(JobHandle scheduledJob, TResult output)
        {
            this.scheduledJob = scheduledJob;
            this.output = output;
        }

        public TResult Complete()
        {
            scheduledJob.Complete();
            return output;
        }
    }
}
