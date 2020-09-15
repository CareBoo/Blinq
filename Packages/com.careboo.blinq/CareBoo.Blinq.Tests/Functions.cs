using CareBoo.Blinq;
using Unity.Mathematics;

internal static class ValueFuncs
{
    public readonly static ValueFunc<int, int>.Impl<Functions.ReturnSelf> ReturnSelf =
        ValueFunc<int, int>.CreateImpl<Functions.ReturnSelf>();

    public readonly static ValueFunc<int, bool>.Impl<Functions.EqualsOne> EqualsOne =
        ValueFunc<int, bool>.CreateImpl<Functions.EqualsOne>();

    public readonly static ValueFunc<int, bool>.Impl<Functions.EqualsZero> EqualsZero =
        ValueFunc<int, bool>.CreateImpl<Functions.EqualsZero>();

    public readonly static ValueFunc<int, int, bool>.Impl<Functions.EqualToIndex> EqualToIndex =
        ValueFunc<int, int, bool>.CreateImpl<Functions.EqualToIndex>();

    public readonly static ValueFunc<int, int, long>.Impl<Functions.AddToIndex> AddToIndex =
        ValueFunc<int, int, long>.CreateImpl<Functions.AddToIndex>();

    public readonly static ValueFunc<int, int, int>.Impl<Functions.Sum> Sum =
        ValueFunc<int, int, int>.CreateImpl<Functions.Sum>();

    public readonly static ValueFunc<long, int, long>.Impl<Functions.LongSum> LongSum =
        ValueFunc<long, int, long>.CreateImpl<Functions.LongSum>();

    public readonly static ValueFunc<long, double>.Impl<Functions.LongToDouble> LongToDouble =
        ValueFunc<long, double>.CreateImpl<Functions.LongToDouble>();

    public readonly static ValueFunc<int, long>.Impl<Functions.IntToLong> IntToLong =
        ValueFunc<int, long>.CreateImpl<Functions.IntToLong>();
}

internal static class Functions
{
    public struct ReturnSelf : IFunc<int, int>
    {
        public int Invoke(int x) => x;
    }

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
