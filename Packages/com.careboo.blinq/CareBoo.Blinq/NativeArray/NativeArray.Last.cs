using Unity.Collections;
using CareBoo.Burst.Delegates;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static T Last<T, TPredicate>(
            this in NativeArray<T> source,
            in ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            for (var i = source.Length - 1; i > -1; i--)
            {
                var val = source[i];
                if (predicate.Invoke(val))
                    return val;
            }
            throw Error.NoMatch();
        }

        public static T LastOrDefault<T, TPredicate>(
            this in NativeArray<T> source,
            in ValueFunc<T, bool>.Struct<TPredicate> predicate,
            in T defaultVal = default
            )
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            for (var i = source.Length - 1; i > -1; i--)
            {
                var val = source[i];
                if (predicate.Invoke(val))
                    return val;
            }
            return defaultVal;
        }

        public static T Last<T>(
            this in NativeArray<T> source
            )
            where T : struct
        {
            return source[source.Length - 1];
        }

        public static T LastOrDefault<T>(
            this in NativeArray<T> source,
            in T defaultVal = default
            )
            where T : struct
        {
            if (source.Length == 0)
                return defaultVal;
            return source[source.Length - 1];
        }
    }
}
