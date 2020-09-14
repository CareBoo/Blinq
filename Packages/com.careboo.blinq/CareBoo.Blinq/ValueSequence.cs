using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public interface ISequence<T>
        where T : struct
    {
        NativeList<T> Execute();
    }

    [BurstCompile]
    public struct ValueSequence<T, TSource>
        : IEnumerable<T>
        , ISequence<T>
        where T : struct
        where TSource : struct, ISequence<T>
    {
        public readonly TSource Source;

        public ValueSequence(TSource source)
        {
            Source = source;
        }

        public NativeList<T> Execute()
        {
            return Source.Execute();
        }

        public NativeArray<T>.Enumerator GetEnumerator()
        {
            return Execute().GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
