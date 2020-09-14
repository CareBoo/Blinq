namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static T ElementAt<T, TSource>(
            this ref ValueSequence<T, TSource> source,
            int index
            )
            where T : struct
            where TSource : struct, ISequence<T>
        {
            var list = source.Execute();
            return list[index];
        }

        public static T ElementAtOrDefault<T, TSource>(
            this ref ValueSequence<T, TSource> source,
            int index,
            T defaultVal = default
            )
            where T : struct
            where TSource : struct, ISequence<T>
        {
            var list = source.Execute();
            if (index >= 0 && index < list.Length)
                return list[index];
            return defaultVal;
        }
    }
}
