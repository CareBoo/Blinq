using System;
using Unity.Collections;
using CareBoo.Burst.Delegates;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static T Single<T, TPredicate>(
            this ref NativeArray<T> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : unmanaged, IEquatable<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var set = new NativeHashSet<T>(source.Length, Allocator.Temp);
            for (var i = 0; i < source.Length; i++)
            {
                var val = source[i];
                if (predicate.Invoke(val))
                    if (!set.Add(val))
                        throw Error.MoreThanOneMatch();
            }
            var iter = set.GetEnumerator();
            if (iter.MoveNext())
                return iter.Current;
            throw Error.NoMatch();
        }

        public static T SingleOrDefault<T, TPredicate>(
            this ref NativeArray<T> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate,
            T defaultVal = default
            )
            where T : unmanaged, IEquatable<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var set = new NativeHashSet<T>(source.Length, Allocator.Temp);
            for (var i = 0; i < source.Length; i++)
            {
                var val = source[i];
                if (predicate.Invoke(val))
                    if (!set.Add(val))
                        throw Error.MoreThanOneMatch();
            }
            var iter = set.GetEnumerator();
            if (iter.MoveNext())
                return iter.Current;
            return defaultVal;
        }

        public static T Single<T>(
            this ref NativeArray<T> source
            )
            where T : unmanaged, IEquatable<T>
        {
            if (source.Length > 1)
                throw Error.MoreThanOneElement();
            if (source.Length == 0)
                throw Error.NoElements();
            return source[0];
        }

        public static T SingleOrDefault<T>(
            this ref NativeArray<T> source,
            T defaultVal = default
            )
            where T : struct
        {
            if (source.Length > 1)
                throw Error.MoreThanOneElement();
            if (source.Length == 0)
                return defaultVal;
            return source[0];
        }
    }
}
