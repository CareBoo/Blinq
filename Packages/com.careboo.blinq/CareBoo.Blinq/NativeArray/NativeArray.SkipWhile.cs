using Unity.Collections;
using CareBoo.Burst.Delegates;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, SkipWhileIndexSequence<T, NativeArraySequence<T>, TPredicate>> SkipWhile<T, TPredicate>(
            this ref NativeArray<T> source,
            ValueFunc<T, int, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TPredicate : struct, IFunc<T, int, bool>
        {
            return source.ToValueSequence().SkipWhile(predicate);
        }

        public static ValueSequence<T, SkipWhileSequence<T, NativeArraySequence<T>, TPredicate>> SkipWhile<T, TPredicate>(
            this ref NativeArray<T> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            return source.ToValueSequence().SkipWhile(predicate);
        }
    }
}
