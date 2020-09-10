using System;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class NativeArrayExtensions
    {
        public static ValueSequence<T, ValueSequence<T, NativeArraySequence<T>>.AppendSequence> Append<T>(this ref NativeArray<T> source, T item)
            where T : struct, IEquatable<T>
        {
            return source.ToValueSequence().Append(item);
        }
    }
}
