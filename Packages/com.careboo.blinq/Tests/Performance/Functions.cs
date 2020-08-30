using CareBoo.Blinq;

internal struct EqualsOne : IFunc<int, bool>
{
    public bool Invoke(int arg0)
    {
        return arg0 == 1;
    }
}
