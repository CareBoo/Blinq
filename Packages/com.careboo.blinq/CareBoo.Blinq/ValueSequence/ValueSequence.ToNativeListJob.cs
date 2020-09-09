using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;

namespace CareBoo.Blinq
{
    public partial struct ValueSequence<T, TQuery>
        : IEnumerable<T>
        where T : struct
        where TQuery : struct, IQuery<T>
    {
        public NativeListJob ToNativeListJob(NativeList<T> output)
        {
            return new NativeListJob { Sequence = this, Output = output };
        }

        public SequenceExecutionJobWrapper<T, NativeListJob, NativeList<T>> ToNativeListJob(Allocator allocator)
        {
            var output = new NativeList<T>(allocator);
            var job = new NativeListJob { Sequence = this, Output = output };
            return new SequenceExecutionJobWrapper<T, NativeListJob, NativeList<T>>(job, output);
        }

        public struct NativeListJob : IJob
        {
            public ValueSequence<T, TQuery> Sequence;

            [WriteOnly]
            public NativeList<T> Output;

            public void Execute()
            {
                Sequence.ToNativeList(Output);
            }
        }
    }
}
