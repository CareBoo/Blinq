using Unity.Collections;
using CareBoo.Burst.Delegates;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static T First<T, TPredicate>(
            this in NativeArray<T> source,
            in ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            for (var i = 0; i < source.Length; i++)
            {
                var val = source[i];
                if (predicate.Invoke(val))
                    return val;
            }
            throw Error.NoMatch();
        }

        public static T FirstOrDefault<T, TPredicate>(
            this in NativeArray<T> source,
            in ValueFunc<T, bool>.Struct<TPredicate> predicate,
            in T defaultVal = default
            )
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            for (var i = 0; i < source.Length; i++)
            {
                var val = source[i];
                if (predicate.Invoke(val))
                    return val;
            }
            return defaultVal;
        }

        public static T First<T>(
            this in NativeArray<T> source
            )
            where T : struct
        {
            return source[0];
        }

        public static T FirstOrDefault<T>(
            this in NativeArray<T> source,
            in T defaultVal = default
            )
            where T : struct
        {
            if (source.Length == 0)
                return defaultVal;
            return source[0];
        }
    }
}
