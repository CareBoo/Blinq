using System.Collections.Generic;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public partial struct ValueSequence<T, TSource>
        : IEnumerable<T>
        where T : struct
        where TSource : struct, ISequence<T>
    {
        public struct ConcatQuery<TSequence> : ISequence<T>
            where TSequence : struct, ISequence<T>
        {
            public TSequence SecondSequence;
            public TSource Query;

            public NativeList<T> Execute()
            {
                var first = Query.Execute();
                var second = SecondSequence.Execute();
                first.AddRange(second);
                second.Dispose();
                return first;
            }
        }

        public ValueSequence<T, ConcatQuery<TSequence>> Concat<TSequence>(TSequence secondSequence)
            where TSequence : struct, ISequence<T>
        {
            var newQuery = new ConcatQuery<TSequence> { SecondSequence = secondSequence, Query = source };
            return new ValueSequence<T, ConcatQuery<TSequence>>(newQuery);
        }
    }
}
