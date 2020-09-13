using CareBoo.Blinq;
using Unity.Mathematics;

internal static class ValueFuncs
{
    public readonly static ValueFunc<int, bool>.Reference<Functions.EqualsOne> EqualsOne =
        ValueFunc<int, bool>.CreateReference<Functions.EqualsOne>();

    public readonly static ValueFunc<int, bool>.Reference<Functions.EqualsZero> EqualsZero =
        ValueFunc<int, bool>.CreateReference<Functions.EqualsZero>();

    public readonly static ValueFunc<int, int, bool>.Reference<Functions.EqualToIndex> EqualToIndex =
        ValueFunc<int, int, bool>.CreateReference<Functions.EqualToIndex>();

    public readonly static ValueFunc<int, int, long>.Reference<Functions.AddToIndex> AddToIndex =
        ValueFunc<int, int, long>.CreateReference<Functions.AddToIndex>();

    public readonly static ValueFunc<int, int, int>.Reference<Functions.Sum> Sum =
        ValueFunc<int, int, int>.CreateReference<Functions.Sum>();

    public readonly static ValueFunc<long, int, long>.Reference<Functions.LongSum> LongSum =
        ValueFunc<long, int, long>.CreateReference<Functions.LongSum>();

    public readonly static ValueFunc<long, double>.Reference<Functions.LongToDouble> LongToDouble =
        ValueFunc<long, double>.CreateReference<Functions.LongToDouble>();

    public readonly static ValueFunc<int, long>.Reference<Functions.IntToLong> IntToLong =
        ValueFunc<int, long>.CreateReference<Functions.IntToLong>();
}

internal static class Functions
{
    public struct EqualsOne : IFunc<int, bool>
    {
        public bool Invoke(int x)
        {
            return x == 1;
        }
    }

    public struct EqualsZero : IFunc<int, bool>
    {
        public bool Invoke(int x)
        {
            return x == 0;
        }
    }

    public struct EqualToIndex : IFunc<int, int, bool>
    {
        public bool Invoke(int x, int i) => x == i;
    }

    public struct AddToIndex : IFunc<int, int, long>
    {
        public long Invoke(int x, int i) => math.aslong(x) + math.aslong(i);
    }

    public struct Sum : IFunc<int, int, int>
    {
        public int Invoke(int acc, int x) => acc + x;
    }

    public struct LongSum : IFunc<long, int, long>
    {
        public long Invoke(long acc, int x) => acc + x;
    }

    public struct LongToDouble : IFunc<long, double>
    {
        public double Invoke(long x) => math.asdouble(x);
    }

    public struct IntToLong : IFunc<int, long>
    {
        public long Invoke(int x) => math.aslong(x);
    }
}
