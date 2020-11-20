using Unity.Collections;
using Unity.Mathematics;
using CareBoo.Burst.Delegates;
using CareBoo.Blinq;

internal static class ValueFuncs
{
    public static ValueFunc<T, T>.Struct<Functions.SelectSelf<T>> SelectSelf<T>() where T : struct =>
        ValueFunc<T, T>.New<Functions.SelectSelf<T>>();

    public readonly static ValueFunc<JoinA, int>.Struct<Functions.JoinAKeySelector> JoinAKeySelector =
        ValueFunc<JoinA, int>.New<Functions.JoinAKeySelector>();

    public readonly static ValueFunc<JoinB, int>.Struct<Functions.JoinBKeySelector> JoinBKeySelector =
        ValueFunc<JoinB, int>.New<Functions.JoinBKeySelector>();

    public readonly static ValueFunc<JoinA, JoinB, JointAB>.Struct<Functions.JointABSelector> JointABSelector =
        ValueFunc<JoinA, JoinB, JointAB>.New<Functions.JointABSelector>();

    public readonly static ValueFunc<JoinA, NativeArray<JoinB>, int>.Struct<Functions.GroupJoinABSelector> GroupJoinABSelector =
        ValueFunc<JoinA, NativeArray<JoinB>, int>.New<Functions.GroupJoinABSelector>();

    public readonly static ValueFunc<int, int>.Struct<Functions.ReturnSelf> ReturnSelf =
        ValueFunc<int, int>.New<Functions.ReturnSelf>();

    public readonly static ValueFunc<int, bool>.Struct<Functions.EqualsOne> EqualsOne =
        ValueFunc<int, bool>.New<Functions.EqualsOne>();

    public readonly static ValueFunc<int, bool>.Struct<Functions.EqualsZero> EqualsZero =
        ValueFunc<int, bool>.New<Functions.EqualsZero>();

    public readonly static ValueFunc<int, bool>.Struct<Functions.GreaterThanOrEqualToZero> GreaterThanOrEqualToZero =
        ValueFunc<int, bool>.New<Functions.GreaterThanOrEqualToZero>();

    public readonly static ValueFunc<int, int, bool>.Struct<Functions.EqualToIndex> EqualToIndex =
        ValueFunc<int, int, bool>.New<Functions.EqualToIndex>();

    public readonly static ValueFunc<int, int, long>.Struct<Functions.AddToIndex> AddToIndex =
        ValueFunc<int, int, long>.New<Functions.AddToIndex>();

    public readonly static ValueFunc<int, int, int>.Struct<Functions.Sum> Sum =
        ValueFunc<int, int, int>.New<Functions.Sum>();

    public readonly static ValueFunc<long, int, long>.Struct<Functions.LongSum> LongSum =
        ValueFunc<long, int, long>.New<Functions.LongSum>();

    public readonly static ValueFunc<long, double>.Struct<Functions.LongToDouble> LongToDouble =
        ValueFunc<long, double>.New<Functions.LongToDouble>();

    public readonly static ValueFunc<int, long>.Struct<Functions.IntToLong> IntToLong =
        ValueFunc<int, long>.New<Functions.IntToLong>();

    public static readonly ValueFunc<Order, int>.Struct<Functions.SelectFirst> SelectFirst =
        ValueFunc<Order, int>.New<Functions.SelectFirst>();

    public static readonly ValueFunc<Order, int>.Struct<Functions.SelectSecond> SelectSecond =
        ValueFunc<Order, int>.New<Functions.SelectSecond>();

    public static readonly ValueFunc<int, NativeArray<int>>.Struct<Functions.RepeatAmount> RepeatAmount =
        ValueFunc<int, NativeArray<int>>.New<Functions.RepeatAmount>();

    public static readonly ValueFunc<int, int, NativeArray<int>>.Struct<Functions.RepeatAmountPlusIndex> RepeatAmountPlusIndex =
        ValueFunc<int, int, NativeArray<int>>.New<Functions.RepeatAmountPlusIndex>();

    internal readonly static ValueFunc<int, ValueGroupingValues<int, int>, int>.Struct<Functions.SelectGrouping> SelectGrouping =
        ValueFunc<int, ValueGroupingValues<int, int>, int>.New<Functions.SelectGrouping>();
}

internal static class Functions
{
    internal struct SelectGrouping : IFunc<int, ValueGroupingValues<int, int>, int>
    {
        public int Invoke(int arg0, ValueGroupingValues<int, int> arg1)
        {
            var list = arg1.ToNativeList(Allocator.Temp);
            var result = arg0 + list.Length;
            list.Dispose();
            return result;
        }
    }

    public struct SelectSelf<T> : IFunc<T, T>
        where T : struct
    {
        public T Invoke(T x) => x;
    }

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

    public struct GreaterThanOrEqualToZero : IFunc<int, bool>
    {
        public bool Invoke(int x) => x >= 0;
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
