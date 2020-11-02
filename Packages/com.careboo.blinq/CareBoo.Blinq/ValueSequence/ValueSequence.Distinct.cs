﻿using System;
using System.Collections;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, DistinctSequence<T, TSource>> Distinct<T, TSource>(
            this ValueSequence<T, TSource> source
            )
            where T : unmanaged, IEquatable<T>
            where TSource : struct, ISequence<T>
        {
            var seq = new DistinctSequence<T, TSource>(source.Source);
            return ValueSequence<T>.New(seq);
        }
    }

    public struct DistinctSequence<T, TSource> : ISequence<T>
        where T : unmanaged, IEquatable<T>
        where TSource : struct, ISequence<T>
    {
        readonly TSource source;

        NativeHashSet<T> set;

        public DistinctSequence(TSource source)
        {
            this.source = source;
            set = default;
        }

        public T Current => source.Current;

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            if (set.IsCreated)
                set.Dispose();
            set = default;
        }

        public bool MoveNext()
        {
            if (!set.IsCreated)
                set = new NativeHashSet<T>(1, Allocator.Persistent);
            while (source.MoveNext())
                if (set.Add(source.Current))
                    return true;
            return false;
        }

        public void Reset()
        {
            throw new NotSupportedException();
        }

        public NativeList<T> ToList()
        {
            var list = source.ToList();
            using (var tempSet = new NativeHashSet<T>(list.Length, Allocator.Temp))
            {
                for (var i = 0; i < list.Length; i++)
                    if (!tempSet.Add(list[i]))
                    {
                        list.RemoveAt(i);
                        i--;
                    }
                return list;
            }
        }
    }
}
