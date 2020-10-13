using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, SkipWhileIndexSequence<T, NativeArraySequence<T>, TPredicate>> SkipWhile<T, TPredicate>(
            this ref NativeArray<T> source,
            ValueFunc<T, int, bool>.Impl<TPredicate> predicate
            )
            where T : struct
            where TPredicate : struct, IFunc<T, int, bool>
        {
            return source.ToValueSequence().SkipWhile(predicate);
        }

        public static ValueSequence<T, SkipWhileSequence<T, NativeArraySequence<T>, TPredicate>> SkipWhile<T, TPredicate>(
            this ref NativeArray<T> source,
            ValueFunc<T, bool>.Impl<TPredicate> predicate
            )
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            return source.ToValueSequence().SkipWhile(predicate);
        }
    }
}
