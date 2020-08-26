using Unity.Burst;

namespace CareBoo.Blinq.Tests
{
    [BurstCompile]
    public static class Predicates
    {
        [BurstCompile]
        public static bool EqualsZero(int val) => val == 0;
        public static BFunc<int, bool> EqualsZeroFunc = new BFunc<int, bool>(EqualsZero);
    }
}
