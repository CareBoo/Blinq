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
        public readonly TSource Source;

        public ValueSequence(TSource source)
        {
            Source = source;
        }

        public NativeList<T> Execute()
        {
            return Source.ToList();
        }

        public NativeList<T> RunExecute()
        {
            var output = new NativeList<T>(Allocator.Persistent);
            var job = new SequenceExecuteJob<T, TSource> { Source = Source, Output = output };
            job.Run();
            return output;
        }

        public SequenceExecuteJobHandle<T> ScheduleExecute()
        {
            var output = new NativeList<T>(Allocator.Persistent);
            var job = new SequenceExecuteJob<T, TSource> { Source = Source, Output = output };
            return new SequenceExecuteJobHandle<T>(job.Schedule(), output);
        }

        public SequenceEnumerator<T, TSource> GetEnumerator()
        {
            return Source;
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
        public static ValueSequence<T, TSource> New<TSource>(TSource source)
            where TSource : struct, ISequence<T>
        {
            return new ValueSequence<T, TSource>(source);
        }
    }
}
