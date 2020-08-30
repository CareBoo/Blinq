using CareBoo.Blinq;
using Unity.Mathematics;

internal struct EqualsZero : IFunc<int, bool>
{
    public bool Invoke(int arg0) => arg0 == 0;
}

internal struct Sum : IFunc<int, int, int>
{
    public int Invoke(int acc, int val)
    {
        return acc + val;
    }
}

internal struct LongSum : IFunc<long, int, long>
{
    public long Invoke(long acc, int val)
    {
        return acc + math.aslong(val);
    }
}

internal struct ToDouble : IFunc<long, double>
{
    public double Invoke(long val)
    {
        return math.asdouble(val);
    }
}
