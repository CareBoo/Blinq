using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class NativeArrayExtensions
    {
        public static int Count<T, TPredicate>(this ref NativeArray<T> source, TPredicate predicate = default)
            where T : struct
            where TPredicate : struct, IValueFunc<T, bool>
        {
            var count = 0;
            for (var i = 0; i < source.Length; i++)
                if (predicate.Invoke(source[i]))
                    count++;
            return count;
        }

        public static int Count<T>(this ref NativeArray<T> source)
            where T : struct
        {
            return source.Length;
        }
    }
}
