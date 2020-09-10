using Unity.Collections;

namespace CareBoo.Blinq
{
    public partial struct ValueSequence<T, TSource>
    {
        public struct UnionSequence<TSecond> : ISequence<T>
            where TSecond : struct, ISequence<T>
        {
            public TSource Source;
            public TSecond Second;

            public NativeList<T> Execute()
            {
                var sourceList = Source.Execute();
                var secondList = Second.Execute();
                var set = new NativeHashSet<T>(sourceList.Length + secondList.Length, Allocator.Temp);
                for (var i = 0; i < sourceList.Length; i++)
                    set.Add(sourceList[i]);
                for (var i = 0; i < secondList.Length; i++)
                    set.Add(secondList[i]);
                var setArr = set.ToNativeArray(Allocator.Temp);
                sourceList.CopyFrom(setArr);
                secondList.Dispose();
                set.Dispose();
                setArr.Dispose();
                return sourceList;
            }
        }

        public ValueSequence<T, UnionSequence<TSecond>> Union<TSecond>(TSecond second)
            where TSecond : struct, ISequence<T>
        {
            var newSequence = new UnionSequence<TSecond> { Source = source, Second = second };
            return new ValueSequence<T, UnionSequence<TSecond>>(newSequence);
        }

        public ValueSequence<T, UnionSequence<ValueSequence<T, NativeArraySequence<T>>>> Union(NativeArray<T> second)
        {
            var newSequence = new UnionSequence<ValueSequence<T, NativeArraySequence<T>>> { Source = source, Second = second.ToValueSequence() };
            return new ValueSequence<T, UnionSequence<ValueSequence<T, NativeArraySequence<T>>>>(newSequence);
        }
    }
}
