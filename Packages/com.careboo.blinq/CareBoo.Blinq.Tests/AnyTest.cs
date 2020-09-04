using NUnit.Framework;
using Linq = System.Linq.Enumerable;
using static Utils;
using Unity.Collections;
using Blinq = CareBoo.Blinq.NativeArrayExtensions;

internal class AnyTest
{
    [Test, Parallelizable]
    public void BlinqShouldEqualLinqNativeArrayAny([NativeArrayValues] NativeArray<int> source)
    {
        var expected = ExceptionOrValue(() => Linq.Any(source));
        var actual = ExceptionOrValue(() => Blinq.Any(ref source));
        Assert.AreEqual(expected, actual);
        source.Dispose();
    }

    [Test, Parallelizable]
    public void BlinqShouldEqualLinqNativeArrayAnyPredicate([NativeArrayValues] NativeArray<int> source)
    {
        var expected = ExceptionOrValue(() => Linq.Any(source, default(EqualsZero).Invoke));
        var actual = ExceptionOrValue(() => Blinq.Any<int, EqualsZero>(ref source));
        Assert.AreEqual(expected, actual);
        source.Dispose();
    }
}
