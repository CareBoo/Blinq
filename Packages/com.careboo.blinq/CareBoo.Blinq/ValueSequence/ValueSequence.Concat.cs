﻿using Unity.Collections;

namespace CareBoo.Blinq
{
    public partial struct ValueSequence<T, TSource>
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
            return Create(newSequence);
        }
    }
}
