using Unity.Collections;

namespace CareBoo.Blinq
{
    public partial struct ValueSequence<T, TSource>
    {
        public struct WhereWithIndexSequence : ISequence<T>
        {
            public TSource Source;
            public ValueFunc<T, int, bool> Predicate;

            public NativeList<T> Execute()
            {
                var sourceList = Source.Execute();
                for (var i = 0; i < sourceList.Length; i++)
                {
                    if (!Predicate.Invoke(sourceList[i], i))
                    {
                        sourceList.RemoveAt(i);
                        i--;
                    }
                }
                return sourceList;
            }
        }

        public ValueSequence<T, WhereWithIndexSequence> Where(ValueFunc<T, int, bool> predicate)
        {
            var newSequence = new WhereWithIndexSequence { Source = source, Predicate = predicate };
            return Create(newSequence);
        }

        public struct WhereSequence : ISequence<T>
        {
            public TSource Source;
            public ValueFunc<T, bool> Predicate;

            public NativeList<T> Execute()
            {
                var sourceList = Source.Execute();
                for (var i = 0; i < sourceList.Length; i++)
                {
                    if (!Predicate.Invoke(sourceList[i]))
                    {
                        sourceList.RemoveAt(i);
                        i--;
                    }
                }
                return sourceList;
            }
        }

        public ValueSequence<T, WhereSequence> Where(ValueFunc<T, bool> predicate)
        {
            var newSequence = new WhereSequence { Source = source, Predicate = predicate };
            return Create(newSequence);
        }
    }
}
