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
            where TSequence : struct, IValueSequence<T>
        {
            public TSequence SecondSequence;
            public TQuery Query;

            public NativeList<T> Execute()
            {
                var first = Query.Execute();
                var second = SecondSequence.ToNativeList();
                first.AddRange(second);
                return first;
            }
        }

        public ValueSequence<T, ConcatQuery<TSequence>> Concat<TSequence>(TSequence secondSequence)
            where TSequence : struct, IValueSequence<T>
        {
            var newQuery = new ConcatQuery<TSequence> { SecondSequence = secondSequence, Query = query };
            return new ValueSequence<T, ConcatQuery<TSequence>>(newQuery);
        }
    }
}
