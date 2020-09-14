using System;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static T Single<T, TSource, TPredicate>(
            this ref ValueSequence<T, TSource> source,
            ValueFunc<T, bool>.Impl<TPredicate> predicate
            )
            where T : unmanaged, IEquatable<T>
            where TSource : struct, ISequence<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var list = source.Execute();
            var set = new NativeHashSet<T>(list.Length, Allocator.Temp);
            for (var i = 0; i < list.Length; i++)
            {
                var val = list[i];
                if (predicate.Invoke(val))
                    if (!set.Add(val))
                        throw Error.MoreThanOneMatch();
            }
            var iter = set.GetEnumerator();
            if (iter.MoveNext())
                return iter.Current;
            throw Error.NoMatch();
        }

        public static T SingleOrDefault<T, TSource, TPredicate>(
            this ref ValueSequence<T, TSource> source,
            ValueFunc<T, bool>.Impl<TPredicate> predicate,
            T defaultVal = default
            )
            where T : unmanaged, IEquatable<T>
            where TSource : struct, ISequence<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var list = source.Execute();
            var set = new NativeHashSet<T>(list.Length, Allocator.Temp);
            for (var i = 0; i < list.Length; i++)
            {
                var val = list[i];
                if (predicate.Invoke(val))
                    if (!set.Add(val))
                        throw Error.MoreThanOneMatch();
            }
            var iter = set.GetEnumerator();
            if (iter.MoveNext())
                return iter.Current;
            return defaultVal;
        }

        public static T Single<T, TSource>(
            this ref ValueSequence<T, TSource> source
            )
            where T : struct
            where TSource : struct, ISequence<T>
        {
            var list = source.Execute();
            if (list.Length > 1)
                throw Error.MoreThanOneElement();
            if (list.Length == 0)
                throw Error.NoElements();
            return list[0];
        }

        public static T SingleOrDefault<T, TSource>(
            this ref ValueSequence<T, TSource> source,
            T defaultVal = default
            )
            where T : struct
            where TSource : struct, ISequence<T>
        {
            var list = source.Execute();
            if (list.Length > 1)
                throw Error.MoreThanOneElement();
            if (list.Length == 0)
                return defaultVal;
            return list[0];
        }
    }
}
