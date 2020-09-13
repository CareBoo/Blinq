namespace CareBoo.Blinq
{
    public partial struct ValueSequence<T, TSource>
    {
        public int Count<TPredicate>(ValueFunc<T, bool>.Impl<TPredicate> predicate)
            where TPredicate : struct, IFunc<T, bool>
        {
            var count = 0;
            var sourceList = source.Execute();
            for (var i = 0; i < sourceList.Length; i++)
                if (predicate.Invoke(sourceList[i]))
                    count++;
            return count;
        }

        public int Count()
        {
            var sourceList = source.Execute();
            return sourceList.Length;
        }
    }
}
