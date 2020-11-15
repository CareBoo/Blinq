using static ValueFuncs;
using static Utils;
using NUnit.Framework;
using Unity.Collections;
using Linq = System.Linq.Enumerable;
using Blinq = CareBoo.Blinq.Sequence;

internal class GroupByTest
{

    [Test, Parallelizable, Timeout(5000)]
    public void BlinqShouldEqualLinqNativeArrayGroupBy([ArrayValues] int[] sourceArr)
    {
        var source = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var expected = ExceptionAndValue(() =>
        {
            var groupBy = Linq.GroupBy(source, SelectSelf<int>().Invoke, (key, values) => key + Linq.Count(values));
            return Linq.ToArray(groupBy);
        });
        var actual = ExceptionAndValue(() =>
        {
            var groupBy = Blinq.GroupBy(source, SelectSelf<int>(), SelectGrouping);
            return Linq.ToArray(groupBy);
        });
        AssertAreEqual(expected, actual);
        source.Dispose();
    }
}
