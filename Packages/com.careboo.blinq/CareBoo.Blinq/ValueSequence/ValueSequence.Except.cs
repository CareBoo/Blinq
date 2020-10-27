using System;
using System.Collections;
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
            return ValueSequence<T>.New(seq);
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

        NativeHashSet<T> set;

        public T Current => Source.Current;

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            Source.Dispose();
            Second.Dispose();
        }

        public bool MoveNext()
        {
            if (!set.IsCreated)
            {
                using (var secondList = Second.ToList())
                {
                    set = new NativeHashSet<T>(secondList.Length, Allocator.Persistent);
                    for (var i = 0; i < secondList.Length; i++)
                        set.Add(secondList[i]);
                }
            }
            while (Source.MoveNext())
                if (!set.Contains(Source.Current))
                    return true;
            return false;
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public NativeList<T> ToList()
        {
            var source = Source.ToList();
            using (var second = Second.ToList())
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
