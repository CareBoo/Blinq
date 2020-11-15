using CareBoo.Burst.Delegates;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static T Last<T, TSource, TPredicate>(
            this in ValueSequence<T, TSource> source,
            in ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var list = source.ToNativeList(Allocator.Temp);
            for (var i = list.Length - 1; i > -1; i--)
            {
                var val = list[i];
                if (predicate.Invoke(val))
                {
                    list.Dispose();
                    return val;
                }
            }
            list.Dispose();
            throw Error.NoMatch();
        }

        public static T LastOrDefault<T, TSource, TPredicate>(
            this in ValueSequence<T, TSource> source,
            in ValueFunc<T, bool>.Struct<TPredicate> predicate,
            in T defaultVal = default
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var list = source.ToNativeList(Allocator.Temp);
            for (var i = list.Length - 1; i > -1; i--)
            {
                var val = list[i];
                if (predicate.Invoke(val))
                {
                    list.Dispose();
                    return val;
                }
            }
            list.Dispose();
            return defaultVal;
        }

        public static T Last<T, TSource>(
            this in ValueSequence<T, TSource> source
            )
            where T : struct
            where TSource : struct, ISequence<T>
        {
            var list = source.ToNativeList(Allocator.Temp);
            if (list.Length == 0)
            {
                list.Dispose();
                throw Error.NoElements();
            }
            var result = list[list.Length - 1];
            list.Dispose();
            return result;
        }

        public static T LastOrDefault<T, TSource>(
            this in ValueSequence<T, TSource> source,
            in T defaultVal = default
            )
            where T : struct
            where TSource : struct, ISequence<T>
        {
            var list = source.ToNativeList(Allocator.Temp);
            if (list.Length == 0)
            {
                list.Dispose();
                return defaultVal;
            }
            var result = list[list.Length - 1];
            list.Dispose();
            return result;
        }
    }
}
