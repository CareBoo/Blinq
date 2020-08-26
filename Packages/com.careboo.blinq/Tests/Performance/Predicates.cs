using CareBoo.Blinq;

internal struct EqualsZero : IPredicate<int>
{
    public bool Invoke(int val)
    {
        return val == 0;
    }
}

internal struct EqualsOne : IPredicate<int>
{
    public bool Invoke(int val)
    {
        return val == 1;
    }
}
