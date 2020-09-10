using System;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public partial struct ValueSequence<T, TSource>
        where T : unmanaged, IEquatable<T>
        where TSource : struct, ISequence<T>
    {
        public struct ConcatSequence<TSecond> : ISequence<T>
            where TSecond : struct, ISequence<T>
        {
            public TSource Source;
            public TSecond Second;

            public NativeList<T> Execute()
            {
                var first = Source.Execute();
                var second = Second.Execute();
                first.AddRange(second);
                second.Dispose();
                return first;
            }
        }

        public ValueSequence<T, ConcatSequence<TSecond>> Concat<TSecond>(TSecond second)
            where TSecond : struct, ISequence<T>
        {
            var newSequence = new ConcatSequence<TSecond> { Source = source, Second = second };
            return new ValueSequence<T, ConcatSequence<TSecond>>(newSequence);
        }
    }
}
