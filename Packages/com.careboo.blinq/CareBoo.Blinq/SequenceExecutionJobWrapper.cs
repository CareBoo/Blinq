using System.Collections.Generic;
using Unity.Jobs;

namespace CareBoo.Blinq
{
    public struct SequenceExecutionJobWrapper<T, TJob, TResult>
        where T : struct
        where TJob : struct, IJob
        where TResult : struct, IEnumerable<T>
    {
        readonly TJob job;
        readonly TResult output;

        public SequenceExecutionJobWrapper(TJob job, TResult output)
        {
            this.job = job;
            this.output = output;
        }

        public SequenceExecutionJobHandle<T, TResult> Schedule()
        {
            var scheduledJob = job.Schedule();
            return new SequenceExecutionJobHandle<T, TResult>(scheduledJob, output);
        }

        public TResult Run()
        {
            job.Run();
            return output;
        }
    }
}
