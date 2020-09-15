namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static T First<T, TSource, TPredicate>(
            this ValueSequence<T, TSource> source,
            ValueFunc<T, bool>.Impl<TPredicate> predicate
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var list = source.Execute();
            for (var i = 0; i < list.Length; i++)
            {
                var val = list[i];
                if (predicate.Invoke(val))
                    return val;
            }
            throw Error.NoMatch();
        }

        public static T FirstOrDefault<T, TSource, TPredicate>(
            this ValueSequence<T, TSource> source,
            ValueFunc<T, bool>.Impl<TPredicate> predicate,
            T defaultVal = default
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var list = source.Execute();
            for (var i = 0; i < list.Length; i++)
            {
                var val = list[i];
                if (predicate.Invoke(val))
                    return val;
            }
            return defaultVal;
        }

        public static T First<T, TSource>(
            this ValueSequence<T, TSource> source
            )
            where T : struct
            where TSource : struct, ISequence<T>
        {
            var list = source.Execute();
            return list[0];
        }

        public static T FirstOrDefault<T, TSource>(
            this ValueSequence<T, TSource> source,
            T defaultVal = default
            )
            where T : struct
            where TSource : struct, ISequence<T>
        {
            var list = source.Execute();
            if (list.Length == 0)
                return defaultVal;
            return list[0];
        }
    }
}
