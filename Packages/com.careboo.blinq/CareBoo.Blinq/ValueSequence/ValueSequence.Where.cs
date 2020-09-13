using Unity.Collections;

namespace CareBoo.Blinq
{
    public partial struct ValueSequence<T, TSource>
    {
        public struct WhereWithIndexSequence<TPredicate> : ISequence<T>
            where TPredicate : struct, IFunc<T, int, bool>
        {
            public TSource Source;
            public ValueFunc<T, int, bool>.Impl<TPredicate> Predicate;

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

        public ValueSequence<T, WhereWithIndexSequence<TPredicate>> Where<TPredicate>(
            ValueFunc<T, int, bool>.Impl<TPredicate> predicate
            )
            where TPredicate : struct, IFunc<T, int, bool>
        {
            var newSequence = new WhereWithIndexSequence<TPredicate> { Source = source, Predicate = predicate };
            return Create(newSequence);
        }

        public struct WhereSequence<TPredicate> : ISequence<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            public TSource Source;
            public ValueFunc<T, bool>.Impl<TPredicate> Predicate;

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

        public ValueSequence<T, WhereSequence<TPredicate>> Where<TPredicate>(ValueFunc<T, bool>.Impl<TPredicate> predicate)
            where TPredicate : struct, IFunc<T, bool>
        {
            var newSequence = new WhereSequence<TPredicate> { Source = source, Predicate = predicate };
            return Create(newSequence);
        }
    }
}
