using NUnit.Framework;
using Linq = System.Linq.Enumerable;
using static Utils;
using Unity.Collections;
using Blinq = CareBoo.Blinq.Sequence;

internal class ElementAtTest
{
    [Test, Parallelizable, Timeout(5000)]
    public void BlinqShouldEqualLinqNativeArrayElementAt([ArrayValues] int[] sourceArr)
    {
        var index = 10;
        var source = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var expected = ExceptionAndValue(() => Linq.ElementAt(source, index));
        var actual = ExceptionAndValue(() => Blinq.ElementAt(source, index));
        AssertAreEqual(expected, actual);
        source.Dispose();
    }

    [Test, Parallelizable, Timeout(5000)]
    public void BlinqShouldEqualLinqNativeArrayElementAtOrDefault([ArrayValues] int[] sourceArr)
    {
        var index = 10;
        var source = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var expected = ExceptionAndValue(() => Linq.ElementAtOrDefault(source, index));
        var actual = ExceptionAndValue(() => Blinq.ElementAtOrDefault(source, index));
        AssertAreEqual(expected, actual);
        source.Dispose();
    }

    [Test, Parallelizable, Timeout(5000)]
    public void BlinqShouldEqualLinqNativeSequenceElementAt([ArrayValues] int[] sourceArr)
    {
        var index = 10;
        var source = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var seq = Blinq.ToValueSequence(in source);
        var expected = ExceptionAndValue(() => Linq.ElementAt(seq, index));
        var actual = ExceptionAndValue(() => Blinq.ElementAt(seq, index));
        AssertAreEqual(expected, actual);
        source.Dispose();
    }

    [Test, Parallelizable, Timeout(5000)]
    public void BlinqShouldEqualLinqNativeSequenceElementAtOrDefault([ArrayValues] int[] sourceArr)
    {
        var index = 10;
        var source = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var seq = Blinq.ToValueSequence(in source);
        var expected = ExceptionAndValue(() => Linq.ElementAtOrDefault(seq, index));
        var actual = ExceptionAndValue(() => Blinq.ElementAtOrDefault(seq, index));
        AssertAreEqual(expected, actual);
        source.Dispose();
    }


    [Test, Parallelizable, Timeout(5000)]
    public void BlinqShouldEqualLinqNativeSequenceRunElementAt([ArrayValues] int[] sourceArr)
    {
        var index = 10;
        using (var source = new NativeArray<int>(sourceArr, Allocator.Persistent))
        {
            var seq = Blinq.ToValueSequence(in source);
            var expected = ExceptionAndValue(() => Linq.ElementAt(seq, index));
            if (expected.exception != null)
                return;
            var actual = ExceptionAndValue(() => Blinq.RunElementAt(seq, index));
            AssertAreEqual(expected, actual);
        }
    }

    [Test, Parallelizable, Timeout(5000)]
    public void BlinqShouldEqualLinqNativeSequenceRunElementAtOrDefault([ArrayValues] int[] sourceArr)
    {
        var index = 10;
        using (var source = new NativeArray<int>(sourceArr, Allocator.Persistent))
        {
            var seq = Blinq.ToValueSequence(in source);
            var expected = ExceptionAndValue(() => Linq.ElementAtOrDefault(seq, index));
            if (expected.exception != null)
                return;
            var actual = ExceptionAndValue(() => Blinq.RunElementAtOrDefault(seq, index));
            AssertAreEqual(expected, actual);
        }
    }


    [Test, Parallelizable, Timeout(5000)]
    public void BlinqShouldEqualLinqNativeSequenceScheduleElementAt([ArrayValues] int[] sourceArr)
    {
        var index = 10;
        using (var source = new NativeArray<int>(sourceArr, Allocator.Persistent))
        {
            var seq = Blinq.ToValueSequence(in source);
            var expected = ExceptionAndValue(() => Linq.ElementAt(seq, index));
            if (expected.exception != null)
                return;
            var actual = ExceptionAndValue(() => Blinq.ScheduleElementAt(seq, index).Complete());
            AssertAreEqual(expected, actual);
        }
    }

    [Test, Parallelizable, Timeout(5000)]
    public void BlinqShouldEqualLinqNativeSequenceScheduleElementAtOrDefault([ArrayValues] int[] sourceArr)
    {
        var index = 10;
        using (var source = new NativeArray<int>(sourceArr, Allocator.Persistent))
        {
            var seq = Blinq.ToValueSequence(in source);
            var expected = ExceptionAndValue(() => Linq.ElementAtOrDefault(seq, index));
            if (expected.exception != null)
                return;
            var actual = ExceptionAndValue(() => Blinq.ScheduleElementAtOrDefault(seq, index).Complete());
            AssertAreEqual(expected, actual);
        }
    }
}
