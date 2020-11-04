namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static T ElementAt<T, TSource>(
            this in ValueSequence<T, TSource> source,
            in int index
            )
            where T : struct
            where TSource : struct, ISequence<T>
        {
            var list = source.Execute();
            var result = list[index];
            list.Dispose();
            return result;
        }

        public static T ElementAtOrDefault<T, TSource>(
            this in ValueSequence<T, TSource> source,
            in int index,
            in T defaultVal = default
            )
            where T : struct
            where TSource : struct, ISequence<T>
        {
            var list = source.Execute();
            var result = index >= 0 && index < list.Length
                ? list[index]
                : defaultVal;
            list.Dispose();
            return result;
        }
    }
}
