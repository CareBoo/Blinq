using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;

namespace CareBoo.Blinq
{
    [BurstCompile]
    public partial struct ValueSequence<T, TQuery>
        : IEnumerable<T>
        , IQuery<T>
        where T : struct
        where TQuery : struct, IQuery<T>
    {
        readonly TQuery query;

        public ValueSequence(TQuery query)
        {
            this.query = query;
        }

        public NativeList<T> Execute()
        {
            return query.Execute();
        }

        public NativeArray<T>.Enumerator GetEnumerator()
        {
            return Execute().GetEnumerator();
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
}
