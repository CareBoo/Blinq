using CareBoo.Burst.Delegates;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static int Count<T, TSource, TPredicate>(
            this in ValueSequence<T, TSource> source,
            in ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var sourceList = source.ToNativeList(Allocator.Temp);
            var count = 0;
            for (var i = 0; i < sourceList.Length; i++)
                if (predicate.Invoke(sourceList[i]))
                    count++;
            sourceList.Dispose();
            return count;
        }

        public static int Count<T, TSource>(this in ValueSequence<T, TSource> source)
            where T : struct
            where TSource : struct, ISequence<T>
        {
            var sourceList = source.ToNativeList(Allocator.Temp);
            var result = sourceList.Length;
            sourceList.Dispose();
            return result;
        }
    }
}
