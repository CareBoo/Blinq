using CareBoo.Blinq;
using Unity.Mathematics;

internal struct EqualsZero : IFunc<int, bool>
{
    public bool Invoke(int arg0) => arg0 == 0;
}

internal struct EqualToIndex : IFunc<int, int, bool>
{
    public bool Invoke(int val, int index) => val == index;
}

internal struct AddToIndex : IFunc<int, int, long>
{
    public long Invoke(int a, int b) => math.aslong(a) + math.aslong(b);
}

internal struct Sum : IFunc<int, int, int>
{
    public int Invoke(int acc, int val) => acc + val;
}

internal struct LongSum : IFunc<long, int, long>
{
    public long Invoke(long acc, int val) => acc + math.aslong(val);
}

internal struct LongToDouble : IFunc<long, double>
{
    public double Invoke(long val) => math.asdouble(val);
}

internal struct IntToLong : IFunc<int, long>
{
    public long Invoke(int val) => math.aslong(val);
}
