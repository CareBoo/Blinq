namespace CareBoo.Blinq
{
    public partial struct ValueSequence<T, TSource>
    {
        public bool All(ValueFunc<T, bool> predicate)
        {
            var sourceList = source.Execute();
            for (var i = 0; i < sourceList.Length; i++)
                if (!predicate.Invoke(sourceList[i]))
                    return false;
            return true;
        }
    }
}
