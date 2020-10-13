using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static T Last<T, TPredicate>(
            this ref NativeArray<T> source,
            ValueFunc<T, bool>.Impl<TPredicate> predicate
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
            this ref NativeArray<T> source,
            ValueFunc<T, bool>.Impl<TPredicate> predicate,
            T defaultVal = default
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
            this ref NativeArray<T> source
            )
            where T : struct
        {
            return source[source.Length - 1];
        }

        public static T LastOrDefault<T>(
            this ref NativeArray<T> source,
            T defaultVal = default
            )
            where T : struct
        {
            if (source.Length == 0)
                return defaultVal;
            return source[source.Length - 1];
        }
    }
}
