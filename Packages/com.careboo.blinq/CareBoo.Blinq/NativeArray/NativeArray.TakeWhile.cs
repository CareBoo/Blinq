using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, TakeWhileIndexSequence<T, NativeArraySequence<T>, TPredicate>> TakeWhile<T, TPredicate>(
            this ref NativeArray<T> source,
            ValueFunc<T, int, bool>.Impl<TPredicate> predicate
            )
            where T : struct
            where TPredicate : struct, IFunc<T, int, bool>
        {
            return source.ToValueSequence().TakeWhile(predicate);
        }

        public static ValueSequence<T, TakeWhileSequence<T, NativeArraySequence<T>, TPredicate>> TakeWhile<T, TPredicate>(
            this ref NativeArray<T> source,
            ValueFunc<T, bool>.Impl<TPredicate> predicate
            )
            where T : struct
            where TPredicate : struct, IFunc<T, bool>
        {
            return source.ToValueSequence().TakeWhile(predicate);
        }
    }
}
