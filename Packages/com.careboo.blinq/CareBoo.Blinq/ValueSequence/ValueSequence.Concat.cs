using System.Collections.Generic;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public partial struct ValueSequence<T, TQuery>
        : IEnumerable<T>
        where T : struct
        where TQuery : struct, IQuery<T>
    {
        public struct ConcatQuery<TSequence> : IQuery<T>
            where TSequence : struct, IQuery<T>
        {
            public TSequence SecondSequence;
            public TQuery Query;

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
            where TSequence : struct, IQuery<T>
        {
            var newQuery = new ConcatQuery<TSequence> { SecondSequence = secondSequence, Query = query };
            return new ValueSequence<T, ConcatQuery<TSequence>>(newQuery);
        }
    }
}
