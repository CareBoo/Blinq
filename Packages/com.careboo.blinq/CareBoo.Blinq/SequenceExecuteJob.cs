using Unity.Collections;
using Unity.Jobs;

namespace CareBoo.Blinq
{
    public struct SequenceExecuteJob<T, TSource> : IJob
        where T : struct
        where TSource : struct, ISequence<T>
    {
        public TSource Source;

        [WriteOnly]
        public NativeList<T> Output;

        public void Execute()
        {
            Output.CopyFrom(Source.ToList());
        }
    }

    public struct SequenceExecuteJobHandle<T>
        where T : struct
    {
        readonly JobHandle jobHandle;
        readonly NativeList<T> output;

        internal SequenceExecuteJobHandle(JobHandle jobHandle, NativeList<T> output)
        {
            this.jobHandle = jobHandle;
            this.output = output;
        }

        public NativeList<T> Complete()
        {
            jobHandle.Complete();
            return output;
        }
    }
}
