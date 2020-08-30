using CareBoo.Blinq;

internal struct EqualsZero : IFunc<int, bool>
{
    public bool Invoke(int arg0) => arg0 == 0;
}
