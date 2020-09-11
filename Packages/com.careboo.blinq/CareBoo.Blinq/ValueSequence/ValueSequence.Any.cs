namespace CareBoo.Blinq
{
    public partial struct ValueSequence<T, TSource>
    {
        public bool Any(ValueFunc<T, bool> predicate)
        {
            var sourceList = source.Execute();
            for (var i = 0; i < sourceList.Length; i++)
                if (predicate.Invoke(sourceList[i]))
                    return true;
            return false;
        }

        public bool Any()
        {
            var sourceList = source.Execute();
            return sourceList.Length > 0;
        }
    }
}
