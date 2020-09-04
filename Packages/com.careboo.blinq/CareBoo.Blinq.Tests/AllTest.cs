using NUnit.Framework;
using Linq = System.Linq.Enumerable;
using static Utils;
using Unity.Collections;
using Blinq = CareBoo.Blinq.NativeArrayExtensions;

internal class AllTest
{
    [Test, Parallelizable]
    public void BlinqAllShouldEqualLinqNativeArrayAll([NativeArrayValues] NativeArray<int> source)
    {
        var expected = ExceptionOrValue(() => Linq.All(source, default(EqualsZero).Invoke));
        var actual = ExceptionOrValue(() => Blinq.All<int, EqualsZero>(ref source));
        Assert.AreEqual(expected, actual);
        source.Dispose();
    }
}
