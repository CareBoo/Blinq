namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static T Last<T, TSource, TPredicate>(
            this ValueSequence<T, TSource> source,
            ValueFunc<T, bool>.Impl<TPredicate> predicate
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var list = source.Execute();
            for (var i = list.Length - 1; i > -1; i--)
            {
                var val = list[i];
                if (predicate.Invoke(val))
                    return val;
            }
            throw Error.NoMatch();
        }

        public static T LastOrDefault<T, TSource, TPredicate>(
            this ValueSequence<T, TSource> source,
            ValueFunc<T, bool>.Impl<TPredicate> predicate,
            T defaultVal = default
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var list = source.Execute();
            for (var i = list.Length - 1; i > -1; i--)
            {
                var val = list[i];
                if (predicate.Invoke(val))
                    return val;
            }
            return defaultVal;
        }

        public static T Last<T, TSource>(
            this ValueSequence<T, TSource> source
            )
            where T : struct
            where TSource : struct, ISequence<T>
        {
            var list = source.Execute();
            return list[list.Length - 1];
        }

        public static T LastOrDefault<T, TSource>(
            this ValueSequence<T, TSource> source,
            T defaultVal = default
            )
            where T : struct
            where TSource : struct, ISequence<T>
        {
            var list = source.Execute();
            if (list.Length == 0)
                return defaultVal;
            return list[list.Length - 1];
        }
    }
}
