using System.Collections.Generic;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public partial struct ValueSequence<T, TSource>
        : IEnumerable<T>
        where T : struct
        where TSource : struct, ISequence<T>
    {
        public struct AppendSequence : ISequence<T>
        {
            public TSource Source;
            public T Item;

            public NativeList<T> Execute()
            {
                var sourceList = Source.Execute();
                sourceList.Add(Item);
                return sourceList;
            }
        }

        public ValueSequence<T, AppendSequence> Append(T item)
        {
            var newSequence = new AppendSequence { Source = source, Item = item };
            return new ValueSequence<T, AppendSequence>(newSequence);
        }
    }
}
