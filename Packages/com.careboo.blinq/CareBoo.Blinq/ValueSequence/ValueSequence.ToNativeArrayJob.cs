using System;
using Unity.Collections;
using Unity.Jobs;

namespace CareBoo.Blinq
{
    public partial struct ValueSequence<T, TSource>
        where T : struct, IEquatable<T>
        where TSource : struct, ISequence<T>
    {
        public NativeArrayJobWrapper ToNativeArrayJob(Allocator allocator)
        {
            var jobOutput = new NativeList<T>(allocator);
            var job = new NativeListJob { Source = this, Output = jobOutput };
            return new NativeArrayJobWrapper(job, jobOutput, allocator);
        }

        public struct NativeArrayJobWrapper
        {
            readonly NativeListJob job;
            readonly NativeList<T> jobOutput;
            readonly Allocator allocator;

            public NativeArrayJobWrapper(NativeListJob job, NativeList<T> jobOutput, Allocator allocator)
            {
                this.job = job;
                this.jobOutput = jobOutput;
                this.allocator = allocator;
            }

            public NativeArrayJobHandle Schedule()
            {
                var scheduledJob = job.Schedule();
                return new NativeArrayJobHandle(scheduledJob, jobOutput, allocator);
            }

            public NativeArray<T> Run()
            {
                job.Run();
                var output = new NativeArray<T>(jobOutput, allocator);
                jobOutput.Dispose();
                return output;
            }
        }

        public struct NativeArrayJobHandle
        {
            readonly JobHandle scheduledJob;
            readonly NativeList<T> jobOutput;
            readonly Allocator allocator;

            public bool IsCompleted => scheduledJob.IsCompleted;

            public NativeArrayJobHandle(JobHandle scheduledJob, NativeList<T> jobOutput, Allocator allocator)
            {
                this.scheduledJob = scheduledJob;
                this.jobOutput = jobOutput;
                this.allocator = allocator;
            }

            public NativeArray<T> Complete()
            {
                scheduledJob.Complete();
                var output = new NativeArray<T>(jobOutput, allocator);
                jobOutput.Dispose();
                return output;
            }
        }
    }
}
