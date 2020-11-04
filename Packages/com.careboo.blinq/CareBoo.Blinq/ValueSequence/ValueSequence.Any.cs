using CareBoo.Burst.Delegates;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static bool Any<T, TSource, TPredicate>(
            this in ValueSequence<T, TSource> source,
            in ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var sourceList = source.Execute();
            for (var i = 0; i < sourceList.Length; i++)
                if (predicate.Invoke(sourceList[i]))
                {
                    sourceList.Dispose();
                    return true;
                }
            sourceList.Dispose();
            return false;
        }

        public static bool Any<T, TSource>(
            this in ValueSequence<T, TSource> source
            )
            where T : struct
            where TSource : struct, ISequence<T>
        {
            var sourceList = source.Execute();
            var result = sourceList.Length > 0;
            sourceList.Dispose();
            return result;
        }
    }
}
