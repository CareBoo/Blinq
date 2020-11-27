﻿using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static T ElementAt<T>(
            this in NativeArray<T> source,
            int index
            )
            where T : struct
        {
            return source[index];
        }

        public static T ElementAtOrDefault<T>(
            this in NativeArray<T> source,
            int index,
            in T defaultVal = default
            )
            where T : struct
        {
            if (index >= 0 && index < source.Length)
                return source[index];
            return defaultVal;
        }
    }
}
