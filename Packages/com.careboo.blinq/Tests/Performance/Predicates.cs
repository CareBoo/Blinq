using CareBoo.Blinq;

internal struct EqualsOne : IFunc<int, bool>, IFunc<int, int, bool>
{
    public bool Invoke(int val)
    {
        return val == 1;
    }

    public bool Invoke(int val, int index)
    {
        return val == 1;
    }
}
