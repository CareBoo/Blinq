using static ValueFuncs;
using static Utils;
using NUnit.Framework;
using Unity.Collections;
using Linq = System.Linq.Enumerable;
using Blinq = CareBoo.Blinq.Sequence;
using CareBoo.Burst.Delegates;

internal class GroupByTest
{
    internal struct SelectGroupingFunc : IFunc<int, NativeMultiHashMap<int, int>, int>
    {
        public int Invoke(int arg0, NativeMultiHashMap<int, int> arg1) => arg0 + arg1.CountValuesForKey(arg0);
    }

    internal readonly static ValueFunc<int, NativeMultiHashMap<int, int>, int>.Struct<SelectGroupingFunc> SelectGrouping =
        ValueFunc<int, NativeMultiHashMap<int, int>, int>.New<SelectGroupingFunc>();

    [Test, Parallelizable]
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
