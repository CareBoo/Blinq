using NUnit.Framework;
using Linq = System.Linq.Enumerable;
using static Utils;
using Unity.Collections;
using Blinq = CareBoo.Blinq.NativeArrayExtensions;

internal class SelectTest
{
    [Test, Parallelizable]
    public void BlinqShouldEqualLinqNativeArraySelectWithIndex([NativeArrayValues] NativeArray<int> source)
    {
        var (expectedException, expectedValue) = ExceptionOrValue(() => Linq.ToArray(Linq.Select(source, default(AddToIndex).Invoke)));
        var (actualException, actualValue) = ExceptionOrValue(() => Linq.ToArray(Blinq.SelectWithIndex<int, long, AddToIndex>(ref source)));
        Assert.AreEqual(expectedException, actualException);
        Assert.AreEqual(expectedValue, actualValue);
        source.Dispose();
    }

    [Test, Parallelizable]
    public void BlinqShouldEqualLinqNativeArraySelect([NativeArrayValues] NativeArray<int> source)
    {
        var (expectedException, expectedValue) = ExceptionOrValue(() => Linq.ToArray(Linq.Select(source, default(IntToLong).Invoke)));
        var (actualException, actualValue) = ExceptionOrValue(() => Linq.ToArray(Blinq.Select<int, long, IntToLong>(ref source)));
        Assert.AreEqual(expectedException, actualException);
        Assert.AreEqual(expectedValue, actualValue);
        source.Dispose();
    }
}
