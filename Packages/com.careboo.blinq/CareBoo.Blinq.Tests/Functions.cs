using CareBoo.Blinq;
using Unity.Burst;
using Unity.Mathematics;

internal static class ValueFuncs
{
    public readonly static ValueFunc<int, bool> EqualsOne =
        ValueFunc<int, bool>.Compile(Functions.EqualsOne);

    public readonly static ValueFunc<int, bool> EqualsZero =
        ValueFunc<int, bool>.Compile(Functions.EqualsZero);

    public readonly static ValueFunc<int, int, bool> EqualToIndex =
        ValueFunc<int, int, bool>.Compile(Functions.EqualToIndex);

    public readonly static ValueFunc<int, int, long> AddToIndex =
        ValueFunc<int, int, long>.Compile(Functions.AddToIndex);

    public readonly static ValueFunc<int, int, int> Sum =
        ValueFunc<int, int, int>.Compile(Functions.Sum);

    public readonly static ValueFunc<long, int, long> LongSum =
        ValueFunc<long, int, long>.Compile(Functions.LongSum);

    public readonly static ValueFunc<long, double> LongToDouble =
        ValueFunc<long, double>.Compile(Functions.LongToDouble);

    public readonly static ValueFunc<int, long> IntToLong =
        ValueFunc<int, long>.Compile(Functions.IntToLong);
}

[BurstCompile]
internal static class Functions
{
    [BurstCompile]
    public static bool EqualsOne(int x) => x == 1;

    [BurstCompile]
    public static bool EqualsZero(int x) => x == 0;

    [BurstCompile]
    public static bool EqualToIndex(int x, int i) => x == i;

    [BurstCompile]
    public static long AddToIndex(int x, int i) => math.aslong(x) + math.aslong(i);

    [BurstCompile]
    public static int Sum(int acc, int x) => acc + x;

    [BurstCompile]
    public static long LongSum(long acc, int x) => acc + x;

    [BurstCompile]
    public static double LongToDouble(long x) => math.asdouble(x);

    [BurstCompile]
    public static long IntToLong(int x) => math.aslong(x);
}
