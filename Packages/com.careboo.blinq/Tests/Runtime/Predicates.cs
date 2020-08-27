using Unity.Burst;

namespace CareBoo.Blinq.Tests
{
    [BurstCompile]
    public static class Predicates
    {
        [BurstCompile]
        public static bool EqualsZero(int val) => val == 0;
        public static BurstCompiledFunc<int, bool> EqualsZeroFunc = BurstCompiledFunc<int, bool>.Compile(EqualsZero);
    }
}
