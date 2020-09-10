using Unity.Collections;
using Unity.Jobs;

namespace CareBoo.Blinq
{
    public partial struct ValueSequence<T, TSource>
    {
        public NativeListJob ToNativeListJob(NativeList<T> output)
        {
            return new NativeListJob { Source = this, Output = output };
        }

        public SequenceExecutionJobWrapper<T, NativeListJob, NativeList<T>> ToNativeListJob(Allocator allocator)
        {
            var output = new NativeList<T>(allocator);
            var job = new NativeListJob { Source = this, Output = output };
            return new SequenceExecutionJobWrapper<T, NativeListJob, NativeList<T>>(job, output);
        }

        public struct NativeListJob : IJob
        {
            public ValueSequence<T, TSource> Source;

            [WriteOnly]
            public NativeList<T> Output;

            public void Execute()
            {
                Source.ToNativeList(Output);
            }
        }
    }
}
