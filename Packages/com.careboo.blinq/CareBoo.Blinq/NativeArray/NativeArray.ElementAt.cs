using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class NativeArrayExtensions
    {
        public static T ElementAt<T>(
            this ref NativeArray<T> source,
            int index
            )
            where T : struct
        {
            return source[index];
        }

        public static T ElementAtOrDefault<T>(
            this ref NativeArray<T> source,
            int index,
            T defaultVal = default
            )
            where T : struct
        {
            if (index >= 0 && index < source.Length)
                return source[index];
            return defaultVal;
        }
    }
}
