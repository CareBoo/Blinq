namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static int Count<T, TSource, TPredicate>(
            this ref ValueSequence<T, TSource> source,
            ValueFunc<T, bool>.Impl<TPredicate> predicate
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var count = 0;
            var sourceList = source.Execute();
            for (var i = 0; i < sourceList.Length; i++)
                if (predicate.Invoke(sourceList[i]))
                    count++;
            return count;
        }

        public static int Count<T, TSource>(this ref ValueSequence<T, TSource> source)
            where T : struct
            where TSource : struct, ISequence<T>
        {
            var sourceList = source.Execute();
            return sourceList.Length;
        }
    }
}
