using Unity.Collections;
using CareBoo.Burst.Delegates;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<
            T,
            SkipWhileSequence<T, NativeArraySequence<T>, NativeArray<T>.Enumerator, TPredicate>,
            SkipWhileSequence<T, NativeArraySequence<T>, NativeArray<T>.Enumerator, TPredicate>.Enumerator>
        SkipWhile<T, TPredicate>(
            this in NativeArray<T> source,
            ValueFunc<T, int, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TPredicate : struct, IFunc<T, int, bool>
        {
            return source.ToValueSequence().SkipWhile(predicate);
        }

        public static ValueSequence<
            T,
            SkipWhileSequence<T, NativeArraySequence<T>, NativeArray<T>.Enumerator, IgnoreIndex<T, bool, TPredicate>>,
            SkipWhileSequence<T, NativeArraySequence<T>, NativeArray<T>.Enumerator, IgnoreIndex<T, bool, TPredicate>>.Enumerator>
        SkipWhile<T, TPredicate>(
            this in NativeArray<T> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            return source.ToValueSequence().SkipWhile(predicate);
        }
    }
}
