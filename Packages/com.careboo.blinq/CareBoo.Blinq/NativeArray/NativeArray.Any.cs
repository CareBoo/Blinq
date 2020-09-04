using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class NativeArrayExtensions
    {
        public static bool Any<T, TPredicate>(this ref NativeArray<T> source, TPredicate predicate = default)
            where T : struct
            where TPredicate : struct, IValueFunc<T, bool>
        {
            for (var i = 0; i < source.Length; i++)
                if (predicate.Invoke(source[i]))
                    return true;
            return false;
        }

        public static bool Any<T>(this ref NativeArray<T> source)
            where T : struct
        {
            return source.Length > 0;
        }
    }
}
