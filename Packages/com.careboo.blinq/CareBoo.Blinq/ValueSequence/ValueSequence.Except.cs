using System;
using System.Collections;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, ExceptSequence<T, TSource, TSecond>> Except<T, TSource, TSecond>(
            this in ValueSequence<T, TSource> source,
            in ValueSequence<T, TSecond> second
            )
            where T : unmanaged, IEquatable<T>
            where TSource : struct, ISequence<T>
            where TSecond : struct, ISequence<T>
        {
            var sourceSeq = source.GetEnumerator();
            var secondSeq = second.GetEnumerator();
            var seq = new ExceptSequence<T, TSource, TSecond>(ref sourceSeq, ref secondSeq);
            return ValueSequence<T>.New(ref seq);
        }

        public static ValueSequence<T, ExceptSequence<T, TSource, NativeArraySequence<T>>> Except<T, TSource>(
            this in ValueSequence<T, TSource> source,
            in NativeArray<T> second
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
        TSource source;
        TSecond second;

        NativeHashSet<T> set;

        public ExceptSequence(ref TSource source, ref TSecond second)
        {
            this.source = source;
            this.second = second;
            set = default;
        }

        public T Current => source.Current;

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            source.Dispose();
            second.Dispose();
            set.Dispose();
        }

        public bool MoveNext()
        {
            if (!set.IsCreated)
            {
                var secondList = second.ToNativeList(Allocator.Temp);
                set = new NativeHashSet<T>(secondList.Length, Allocator.Persistent);
                for (var i = 0; i < secondList.Length; i++)
                    set.Add(secondList[i]);
                secondList.Dispose();
            }
            while (source.MoveNext())
                if (set.Add(source.Current))
                    return true;
            return false;
        }

        public void Reset()
        {
            throw new NotSupportedException();
        }

        public NativeList<T> ToNativeList(Allocator allocator)
        {
            var sourceList = source.ToNativeList(Allocator.Temp);
            var secondList = second.ToNativeList(Allocator.Temp);
            var list = new NativeList<T>(sourceList.Length, allocator);
            var secondSet = new NativeHashSet<T>(secondList.Length, Allocator.Temp);
            for (var i = 0; i < secondList.Length; i++)
                secondSet.Add(secondList[i]);
            for (var i = 0; i < sourceList.Length; i++)
                if (secondSet.Add(sourceList[i]))
                {
                    list.AddNoResize(sourceList[i]);
                }
            sourceList.Dispose();
            secondList.Dispose();
            secondSet.Dispose();
            return list;
        }
    }
}
