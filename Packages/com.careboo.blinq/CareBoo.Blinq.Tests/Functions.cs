using CareBoo.Blinq;
using Unity.Burst;
using Unity.Mathematics;

public static class C
{
    public delegate void DoSomething();
    public static void DoesSomething() { }
    public static FunctionPointer<DoSomething> fptr = BurstCompiler.CompileFunctionPointer<DoSomething>(DoesSomething);
}

internal struct EqualsOne : IValueFunc<int, bool>
{
    public bool Invoke(int arg0) => arg0 == 1;
}

internal struct EqualsZero : IValueFunc<int, bool>
{
    public bool Invoke(int arg0) => arg0 == 0;
}

internal struct EqualToIndex : IValueFunc<int, int, bool>
{
    public bool Invoke(int val, int index) => val == index;
}

internal struct AddToIndex : IValueFunc<int, int, long>
{
    public long Invoke(int a, int b) => math.aslong(a) + math.aslong(b);
}

internal struct Sum : IValueFunc<int, int, int>
{
    public int Invoke(int acc, int val) => acc + val;
}

internal struct LongSum : IValueFunc<long, int, long>
{
    public long Invoke(long acc, int val) => acc + math.aslong(val);
}

internal struct LongToDouble : IValueFunc<long, double>
{
    public double Invoke(long val) => math.asdouble(val);
}

internal struct IntToLong : IValueFunc<int, long>
{
    public long Invoke(int val) => math.aslong(val);
}
