using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public struct SequenceEnumerator<T, TSequence>
        : IEnumerator<T>
        where T : struct
        where TSequence : INativeListConvertible<T>
    {
        NativeList<T> list;
        NativeArray<T>.Enumerator listEnumerator;

        public SequenceEnumerator(
            in TSequence seq
            )
        {
            list = seq.ToNativeList(Allocator.Temp);
            listEnumerator = list.GetEnumerator();
        }

        public T Current => listEnumerator.Current;

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            list.Dispose();
            listEnumerator.Dispose();
        }

        public bool MoveNext()
        {
            return listEnumerator.MoveNext();
        }

        public void Reset()
        {
            throw new NotSupportedException();
        }
    }

    public static class SequenceEnumerator<T>
        where T : struct
    {
        public static SequenceEnumerator<T, TSequence> New<TSequence>(in TSequence sequence)
            where TSequence : struct, INativeListConvertible<T>
        {
            return new SequenceEnumerator<T, TSequence>(in sequence);
        }
    }
}
