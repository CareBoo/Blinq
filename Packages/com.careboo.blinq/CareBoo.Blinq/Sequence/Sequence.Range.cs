using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<int, RangeSequence, RangeSequence.Enumerator> Range(int start, int count)
        {
            var seq = new RangeSequence(in start, in count);
            return ValueSequence<int, RangeSequence.Enumerator>.New(in seq);
        }
    }

    public struct RangeSequence
        : ISequence<int, RangeSequence.Enumerator>
    {
        public struct Enumerator : IEnumerator<int>
        {
            int start;
            int count;

            public Enumerator(int start, int count)
            {
                this.start = start - 1;
                this.count = count;
            }

            public int Current => start;

            object IEnumerator.Current => Current;

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                count -= 1;
                start += 1;
                return count >= 0;
            }

            public void Reset()
            {
                throw new NotSupportedException();
            }
        }

        readonly int start;
        readonly int count;

        public RangeSequence(in int start, in int count)
        {
            this.start = start;
            this.count = count;
        }

        public NativeList<int> ToNativeList(Allocator allocator)
        {
            var list = new NativeList<int>(count, allocator);
            for (var i = 0; i < count; i++)
                list.AddNoResize(i + start);
            return list;
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(start, count);
        }

        IEnumerator<int> IEnumerable<int>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
