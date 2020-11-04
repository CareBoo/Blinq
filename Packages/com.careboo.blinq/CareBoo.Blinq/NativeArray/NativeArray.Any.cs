using Unity.Collections;
using CareBoo.Burst.Delegates;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static bool Any<T, TPredicate>(
            this in NativeArray<T> source,
            in ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            for (var i = 0; i < source.Length; i++)
                if (predicate.Invoke(source[i]))
                    return true;
            return false;
        }

        public static bool Any<T>(this in NativeArray<T> source)
            where T : struct
        {
            return source.Length > 0;
        }
    }
}
