using Unity.Collections;

namespace CareBoo.Blinq
{
    public partial struct ValueSequence<T, TSource>
    {
        public struct DefaultIfEmptySequence : ISequence<T>
        {
            public TSource Source;

            public T Default;

            public NativeList<T> Execute()
            {
                var sourceList = Source.Execute();
                if (sourceList.Length == 0)
                    sourceList.Add(Default);
                return sourceList;
            }
        }

        public ValueSequence<T, DefaultIfEmptySequence> DefaultIfEmpty(T defaultVal = default)
        {
            var newSequence = new DefaultIfEmptySequence { Source = source, Default = defaultVal };
            return Create(newSequence);
        }
    }

}
