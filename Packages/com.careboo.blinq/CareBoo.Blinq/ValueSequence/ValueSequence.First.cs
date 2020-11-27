using System.Collections.Generic;
using CareBoo.Burst.Delegates;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static T First<T, TSource, TSourceEnumerator, TPredicate>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var seq = source.GetEnumerator();
            while (seq.MoveNext())
            {
                var current = seq.Current;
                if (predicate.Invoke(current))
                {
                    seq.Dispose();
                    return current;
                }
            }
            seq.Dispose();
            throw Error.NoMatch();
        }

        public static T FirstOrDefault<T, TSource, TSourceEnumerator, TPredicate>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate,
            in T defaultVal = default
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var seq = source.GetEnumerator();
            while (seq.MoveNext())
            {
                var current = seq.Current;
                if (predicate.Invoke(current))
                {
                    seq.Dispose();
                    return current;
                }
            }
            seq.Dispose();
            return defaultVal;
        }

        public static T First<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var seq = source.GetEnumerator();
            if (!seq.MoveNext())
            {
                seq.Dispose();
                throw Error.NoElements();
            }
            var result = seq.Current;
            seq.Dispose();
            return result;
        }

        public static T FirstOrDefault<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            in T defaultVal = default
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var seq = source.GetEnumerator();
            var result = seq.MoveNext()
                ? seq.Current
                : defaultVal;
            seq.Dispose();
            return result;
        }
    }
}
