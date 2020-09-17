using CareBoo.Blinq;
using Unity.Collections;
using Unity.Mathematics;

internal static class ValueFuncs
{
    public readonly static ValueFunc<JoinA, int>.Impl<Functions.JoinAKeySelector> JoinAKeySelector =
        ValueFunc<JoinA, int>.CreateImpl<Functions.JoinAKeySelector>();

    public readonly static ValueFunc<JoinB, int>.Impl<Functions.JoinBKeySelector> JoinBKeySelector =
        ValueFunc<JoinB, int>.CreateImpl<Functions.JoinBKeySelector>();

    public readonly static ValueFunc<JoinA, JoinB, JointAB>.Impl<Functions.JointABSelector> JointABSelector =
        ValueFunc<JoinA, JoinB, JointAB>.CreateImpl<Functions.JointABSelector>();

    public readonly static ValueFunc<JoinA, NativeArray<JoinB>, int>.Impl<Functions.GroupJoinABSelector> GroupJoinABSelector =
        ValueFunc<JoinA, NativeArray<JoinB>, int>.CreateImpl<Functions.GroupJoinABSelector>();

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

    public static readonly ValueFunc<Order, int>.Impl<Functions.SelectFirst> SelectFirst =
        ValueFunc<Order, int>.CreateImpl<Functions.SelectFirst>();

    public static readonly ValueFunc<Order, int>.Impl<Functions.SelectSecond> SelectSecond =
        ValueFunc<Order, int>.CreateImpl<Functions.SelectSecond>();

    public static readonly ValueFunc<int, NativeArray<int>>.Impl<Functions.RepeatAmount> RepeatAmount =
        ValueFunc<int, NativeArray<int>>.CreateImpl<Functions.RepeatAmount>();

    public static readonly ValueFunc<int, int, NativeArray<int>>.Impl<Functions.RepeatAmountPlusIndex> RepeatAmountPlusIndex =
        ValueFunc<int, int, NativeArray<int>>.CreateImpl<Functions.RepeatAmountPlusIndex>();
}

internal static class Functions
{
    public struct JoinAKeySelector : IFunc<JoinA, int>
    {
        public int Invoke(JoinA x) => x.Id;
    }

    public struct JoinBKeySelector : IFunc<JoinB, int>
    {
        public int Invoke(JoinB x) => x.Id;
    }

    public struct JointABSelector : IFunc<JoinA, JoinB, JointAB>
    {
        public JointAB Invoke(JoinA a, JoinB b) => new JointAB { ValA = a.Val, ValB = b.Val };
    }

    public struct GroupJoinABSelector : IFunc<JoinA, NativeArray<JoinB>, int>
    {
        public int Invoke(JoinA a, NativeArray<JoinB> bArr) => bArr.Length;
    }

    public struct SelectFirst : IFunc<Order, int>
    {
        public int Invoke(Order x) => x.First;
    }

    public struct SelectSecond : IFunc<Order, int>
    {
        public int Invoke(Order x) => x.Second;
    }

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

    public struct RepeatAmount : IFunc<int, NativeArray<int>>
    {
        public NativeArray<int> Invoke(int x)
        {
            var arr = new NativeArray<int>(x, Allocator.Temp);
            for (var i = 0; i < x; i++)
                arr[i] = x;
            return arr;
        }
    }

    public struct RepeatAmountPlusIndex : IFunc<int, int, NativeArray<int>>
    {
        public NativeArray<int> Invoke(int x, int index)
        {
            var arr = new NativeArray<int>(x, Allocator.Temp);
            for (var i = 0; i < x; i++)
                arr[i] = x + index;
            return arr;
        }
    }
}
