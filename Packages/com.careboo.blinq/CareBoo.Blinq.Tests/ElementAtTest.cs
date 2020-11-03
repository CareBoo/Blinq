using NUnit.Framework;
using Linq = System.Linq.Enumerable;
using static Utils;
using Unity.Collections;
using Blinq = CareBoo.Blinq.Sequence;

internal class ElementAtTest
{
    [Test, Parallelizable]
    public void BlinqShouldEqualLinqNativeArrayElementAt([ArrayValues] int[] sourceArr)
    {
        var index = 10;
        var source = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var expected = ExceptionAndValue(() => Linq.ElementAt(source, index));
        var actual = ExceptionAndValue(() => Blinq.ElementAt(source, index));
        AssertAreEqual(expected, actual);
        source.Dispose();
    }

    [Test, Parallelizable]
    public void BlinqShouldEqualLinqNativeArrayElementAtOrDefault([ArrayValues] int[] sourceArr)
    {
        var index = 10;
        var source = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var expected = ExceptionAndValue(() => Linq.ElementAtOrDefault(source, index));
        var actual = ExceptionAndValue(() => Blinq.ElementAtOrDefault(source, index));
        AssertAreEqual(expected, actual);
        source.Dispose();
    }
}
