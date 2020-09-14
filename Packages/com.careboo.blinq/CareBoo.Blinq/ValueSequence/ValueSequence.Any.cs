namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static bool Any<T, TSource, TPredicate>(
            this ref ValueSequence<T, TSource> source,
            ValueFunc<T, bool>.Impl<TPredicate> predicate
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var sourceList = source.Execute();
            for (var i = 0; i < sourceList.Length; i++)
                if (predicate.Invoke(sourceList[i]))
                    return true;
            return false;
        }

        public static bool Any<T, TSource>(
            this ref ValueSequence<T, TSource> source
            )
            where T : struct
            where TSource : struct, ISequence<T>
        {
            var sourceList = source.Execute();
            return sourceList.Length > 0;
        }
    }
}
