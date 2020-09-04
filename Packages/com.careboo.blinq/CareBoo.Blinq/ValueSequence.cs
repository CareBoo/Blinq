using System.Collections;
using System.Collections.Generic;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public interface IValueSequence<T>
        where T : struct
    {
        NativeList<T> ToNativeList();
    }

    public partial struct ValueSequence<T, TQuery>
        : IEnumerable<T>
        , IValueSequence<T>
        where T : struct
        where TQuery : struct, IQuery<T>
    {
        readonly TQuery query;

        public ValueSequence(TQuery query)
        {
            this.query = query;
        }

        public NativeList<T> ToNativeList()
        {
            return query.Execute();
        }

        public NativeArray<T> ToNativeArray()
        {
            return query.Execute().AsArray();
        }

        public NativeArray<T>.Enumerator GetEnumerator()
        {
            return ToNativeList().GetEnumerator();
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
