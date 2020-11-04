using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;

namespace CareBoo.Blinq
{
    public struct ValueSequence<T, TSource>
        : IEnumerable<T>
        where T : struct
        where TSource : struct, ISequence<T>
    {
        TSource source;

        public ValueSequence(ref TSource source)
        {
            this.source = source;
        }

        public NativeList<T> Execute()
        {
            return source.ToList();
        }

        public NativeList<T> RunExecute()
        {
            var output = new NativeList<T>(Allocator.Persistent);
            var job = new SequenceExecuteJob<T, TSource> { Source = source, Output = output };
            job.Run();
            return output;
        }

        public SequenceExecuteJobHandle<T> ScheduleExecute()
        {
            var output = new NativeList<T>(Allocator.Persistent);
            var job = new SequenceExecuteJob<T, TSource> { Source = source, Output = output };
            return new SequenceExecuteJobHandle<T>(job.Schedule(), output);
        }

        public TSource GetEnumerator()
        {
            return source;
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public static class ValueSequence<T>
        where T : struct
    {
        public static ValueSequence<T, TSource> New<TSource>(ref TSource source)
            where TSource : struct, ISequence<T>
        {
            return new ValueSequence<T, TSource>(ref source);
        }
    }
}
