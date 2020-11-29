using NUnit.Framework;
using Linq = System.Linq.Enumerable;
using static Utils;
using Unity.Collections;
using CareBoo.Blinq;
using static ValueFuncs;

internal class LastTest
{
    [Test, Parallelizable, Timeout(5000)]
    public void BlinqShouldEqualLinqNativeArrayLast([ArrayValues] int[] sourceArr)
    {
        var source = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var expected = ExceptionAndValue(() => Linq.Last(source));
        var actual = ExceptionAndValue(() => source.Last());
        AssertAreEqual(expected, actual);
        source.Dispose();
    }

    [Test, Parallelizable, Timeout(5000)]
    public void BlinqShouldEqualLinqNativeArrayLastPredicate([ArrayValues] int[] sourceArr)
    {
        var source = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var expected = ExceptionAndValue(() => Linq.Last(source, EqualsZero.Invoke));
        var actual = ExceptionAndValue(() => source.Last(EqualsZero));
        AssertAreEqual(expected, actual);
        source.Dispose();
    }

    [Test, Parallelizable, Timeout(5000)]
    public void BlinqShouldEqualLinqNativeSequenceLast([ArrayValues] int[] sourceArr)
    {
        using (var sourceNativeArr = new NativeArray<int>(sourceArr, Allocator.Persistent))
        {
            var source = sourceNativeArr.ToValueSequence();
            var expected = ExceptionAndValue(() => Linq.Last(source));
            var actual = ExceptionAndValue(() => source.Last());
            AssertAreEqual(expected, actual);
        }
    }

    [Test, Parallelizable, Timeout(5000)]
    public void BlinqShouldEqualLinqNativeSequenceLastPredicate([ArrayValues] int[] sourceArr)
    {
        using (var sourceNativeArr = new NativeArray<int>(sourceArr, Allocator.Persistent))
        {
            var source = sourceNativeArr.ToValueSequence();
            var expected = ExceptionAndValue(() => Linq.Last(source, EqualsZero.Invoke));
            var actual = ExceptionAndValue(() => source.Last(EqualsZero));
            AssertAreEqual(expected, actual);
        }
    }

    [Test, Parallelizable, Timeout(5000)]
    public void BlinqShouldEqualLinqNativeSequenceRunLast([ArrayValues] int[] sourceArr)
    {
        using (var sourceNativeArr = new NativeArray<int>(sourceArr, Allocator.Persistent))
        {
            var source = sourceNativeArr.ToValueSequence();
            var expected = ExceptionAndValue(() => Linq.Last(source));
            if (expected.exception != null)
                return;
            var actual = ExceptionAndValue(() => source.RunLast());
            AssertAreEqual(expected, actual);
        }
    }

    [Test, Parallelizable, Timeout(5000)]
    public void BlinqShouldEqualLinqNativeSequenceRunLastPredicate([ArrayValues] int[] sourceArr)
    {
        using (var sourceNativeArr = new NativeArray<int>(sourceArr, Allocator.Persistent))
        {
            var source = sourceNativeArr.ToValueSequence();
            var expected = ExceptionAndValue(() => Linq.Last(source, EqualsZero.Invoke));
            if (expected.exception != null)
                return;
            var actual = ExceptionAndValue(() => source.RunLast(EqualsZero));
            AssertAreEqual(expected, actual);
        }
    }

    [Test, Parallelizable, Timeout(5000)]
    public void BlinqShouldEqualLinqNativeSequenceScheduleLast([ArrayValues] int[] sourceArr)
    {
        using (var sourceNativeArr = new NativeArray<int>(sourceArr, Allocator.Persistent))
        {
            var source = sourceNativeArr.ToValueSequence();
            var expected = ExceptionAndValue(() => Linq.Last(source));
            if (expected.exception != null)
                return;
            var actual = ExceptionAndValue(() => source.ScheduleLast().Complete());
            AssertAreEqual(expected, actual);
        }
    }

    [Test, Parallelizable, Timeout(5000)]
    public void BlinqShouldEqualLinqNativeSequenceScheduleLastPredicate([ArrayValues] int[] sourceArr)
    {
        using (var sourceNativeArr = new NativeArray<int>(sourceArr, Allocator.Persistent))
        {
            var source = sourceNativeArr.ToValueSequence();
            var expected = ExceptionAndValue(() => Linq.Last(source, EqualsZero.Invoke));
            if (expected.exception != null)
                return;
            var actual = ExceptionAndValue(() => source.ScheduleLast(EqualsZero).Complete());
            AssertAreEqual(expected, actual);
        }
    }

    [Test, Parallelizable, Timeout(5000)]
    public void BlinqShouldEqualLinqNativeArrayLastOrDefault([ArrayValues] int[] sourceArr)
    {
        var source = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var expected = ExceptionAndValue(() => Linq.LastOrDefault(source));
        var actual = ExceptionAndValue(() => source.LastOrDefault());
        AssertAreEqual(expected, actual);
        source.Dispose();
    }

    [Test, Parallelizable, Timeout(5000)]
    public void BlinqShouldEqualLinqNativeArrayLastOrDefaultPredicate([ArrayValues] int[] sourceArr)
    {
        var source = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var expected = ExceptionAndValue(() => Linq.LastOrDefault(source, EqualsZero.Invoke));
        var actual = ExceptionAndValue(() => source.LastOrDefault(EqualsZero));
        AssertAreEqual(expected, actual);
        source.Dispose();
    }

    [Test, Parallelizable, Timeout(5000)]
    public void BlinqShouldEqualLinqNativeSequenceLastOrDefault([ArrayValues] int[] sourceArr)
    {
        var sourceNativeArr = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var source = sourceNativeArr.ToValueSequence();
        var expected = ExceptionAndValue(() => Linq.LastOrDefault(source));
        var actual = ExceptionAndValue(() => source.LastOrDefault());
        AssertAreEqual(expected, actual);
        sourceNativeArr.Dispose();
    }

    [Test, Parallelizable, Timeout(5000)]
    public void BlinqShouldEqualLinqNativeSequenceLastOrDefaultPredicate([ArrayValues] int[] sourceArr)
    {
        var sourceNativeArr = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var source = sourceNativeArr.ToValueSequence();
        var expected = ExceptionAndValue(() => Linq.LastOrDefault(source, EqualsZero.Invoke));
        var actual = ExceptionAndValue(() => source.LastOrDefault(EqualsZero));
        AssertAreEqual(expected, actual);
        sourceNativeArr.Dispose();
    }

    [Test, Parallelizable, Timeout(5000)]
    public void BlinqShouldEqualLinqNativeSequenceRunLastOrDefault([ArrayValues] int[] sourceArr)
    {
        using (var sourceNativeArr = new NativeArray<int>(sourceArr, Allocator.Persistent))
        {
            var source = sourceNativeArr.ToValueSequence();
            var expected = ExceptionAndValue(() => Linq.Last(source));
            if (expected.exception != null)
                return;
            var actual = ExceptionAndValue(() => source.RunLastOrDefault());
            AssertAreEqual(expected, actual);
        }
    }

    [Test, Parallelizable, Timeout(5000)]
    public void BlinqShouldEqualLinqNativeSequenceRunLastOrDefaultPredicate([ArrayValues] int[] sourceArr)
    {
        using (var sourceNativeArr = new NativeArray<int>(sourceArr, Allocator.Persistent))
        {
            var source = sourceNativeArr.ToValueSequence();
            var expected = ExceptionAndValue(() => Linq.Last(source, EqualsZero.Invoke));
            if (expected.exception != null)
                return;
            var actual = ExceptionAndValue(() => source.RunLastOrDefault(EqualsZero));
            AssertAreEqual(expected, actual);
        }
    }

    [Test, Parallelizable, Timeout(5000)]
    public void BlinqShouldEqualLinqNativeSequenceScheduleLastOrDefault([ArrayValues] int[] sourceArr)
    {
        using (var sourceNativeArr = new NativeArray<int>(sourceArr, Allocator.Persistent))
        {
            var source = sourceNativeArr.ToValueSequence();
            var expected = ExceptionAndValue(() => Linq.Last(source));
            if (expected.exception != null)
                return;
            var actual = ExceptionAndValue(() => source.ScheduleLastOrDefault().Complete());
            AssertAreEqual(expected, actual);
        }
    }

    [Test, Parallelizable, Timeout(5000)]
    public void BlinqShouldEqualLinqNativeSequenceScheduleLastOrDefaultPredicate([ArrayValues] int[] sourceArr)
    {
        using (var sourceNativeArr = new NativeArray<int>(sourceArr, Allocator.Persistent))
        {
            var source = sourceNativeArr.ToValueSequence();
            var expected = ExceptionAndValue(() => Linq.Last(source, EqualsZero.Invoke));
            if (expected.exception != null)
                return;
            var actual = ExceptionAndValue(() => source.ScheduleLastOrDefault(EqualsZero).Complete());
            AssertAreEqual(expected, actual);
        }
    }
}
