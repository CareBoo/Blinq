using System;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, ExceptSequence<T, TSource, TSecond>> Except<T, TSource, TSecond>(
            this ValueSequence<T, TSource> source,
            ValueSequence<T, TSecond> second
            )
            where T : unmanaged, IEquatable<T>
            where TSource : struct, ISequence<T>
            where TSecond : struct, ISequence<T>
        {
            var seq = new ExceptSequence<T, TSource, TSecond> { Source = source.Source, Second = second.Source };
            return new ValueSequence<T, ExceptSequence<T, TSource, TSecond>>(seq);
        }

        public static ValueSequence<T, ExceptSequence<T, TSource, NativeArraySequence<T>>> Except<T, TSource>(
            this ValueSequence<T, TSource> source,
            NativeArray<T> second
            )
            where T : unmanaged, IEquatable<T>
            where TSource : struct, ISequence<T>
        {
            return source.Except(second.ToValueSequence());
        }
    }

    public struct ExceptSequence<T, TSource, TSecond> : ISequence<T>
        where T : unmanaged, IEquatable<T>
        where TSource : struct, ISequence<T>
        where TSecond : struct, ISequence<T>
    {
        public TSource Source;
        public TSecond Second;

        public NativeList<T> Execute()
        {
            var source = Source.Execute();
            using (var second = Second.Execute())
            using (var secondSet = new NativeHashSet<T>(second.Length, Allocator.Temp))
            {
                for (var i = 0; i < second.Length; i++)
                    secondSet.Add(second[i]);
                for (var i = 0; i < source.Length; i++)
                    if (!secondSet.Add(source[i]))
                    {
                        source.RemoveAt(i);
                        i--;
                    }
                return source;
            }
        }
    }
}
