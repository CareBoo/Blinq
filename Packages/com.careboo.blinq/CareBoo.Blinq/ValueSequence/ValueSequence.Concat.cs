﻿using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, ConcatSequence<T, TSource, TSecond>> Concat<T, TSource, TSecond>(
            this ValueSequence<T, TSource> source,
            TSecond second
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TSecond : struct, ISequence<T>
        {
            var newSequence = new ConcatSequence<T, TSource, TSecond> { Source = source.Source, Second = second };
            return new ValueSequence<T, ConcatSequence<T, TSource, TSecond>>(newSequence);
        }

        public static ValueSequence<T, ConcatSequence<T, TSource, NativeArraySequence<T>>> Concat<T, TSource>(
            this ValueSequence<T, TSource> source,
            NativeArray<T> second
            )
            where T : struct
            where TSource : struct, ISequence<T>
        {
            var newSequence = new ConcatSequence<T, TSource, NativeArraySequence<T>> { Source = source.Source, Second = second.ToValueSequence().Source };
            return new ValueSequence<T, ConcatSequence<T, TSource, NativeArraySequence<T>>>(newSequence);
        }

    }

    public struct ConcatSequence<T, TSource, TSecond> : ISequence<T>
        where T : struct
        where TSource : struct, ISequence<T>
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
}
