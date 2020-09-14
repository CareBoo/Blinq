﻿using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class ValueSequenceExtensions
    {
        public static NativeList<T> ToNativeList<T, TSource>(
            this ref ValueSequence<T, TSource> source,
            Allocator allocator
            )
            where T : struct
            where TSource : struct, ISequence<T>
        {
            var list = source.Execute();
            var result = new NativeList<T>(allocator);
            result.CopyFrom(list);
            list.Dispose();
            return result;
        }
    }
}