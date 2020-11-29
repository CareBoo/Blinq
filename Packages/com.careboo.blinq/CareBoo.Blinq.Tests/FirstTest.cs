using NUnit.Framework;
using Linq = System.Linq.Enumerable;
using static Utils;
using Unity.Collections;
using CareBoo.Blinq;
using static ValueFuncs;

internal class FirstTest
{
    [Test, Parallelizable, Timeout(5000)]
    public void BlinqShouldEqualLinqNativeArrayFirst([ArrayValues] int[] sourceArr)
    {
        var source = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var expected = ExceptionAndValue(() => Linq.First(source));
        var actual = ExceptionAndValue(() => source.First());
        AssertAreEqual(expected, actual);
        source.Dispose();
    }

    [Test, Parallelizable, Timeout(5000)]
    public void BlinqShouldEqualLinqNativeArrayFirstPredicate([ArrayValues] int[] sourceArr)
    {
        var source = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var expected = ExceptionAndValue(() => Linq.First(source, EqualsZero.Invoke));
        var actual = ExceptionAndValue(() => source.First(EqualsZero));
        AssertAreEqual(expected, actual);
        source.Dispose();
    }

    [Test, Parallelizable, Timeout(5000)]
    public void BlinqShouldEqualLinqNativeSequenceFirst([ArrayValues] int[] sourceArr)
    {
        var sourceNativeArr = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var source = sourceNativeArr.ToValueSequence();
        var expected = ExceptionAndValue(() => Linq.First(source));
        var actual = ExceptionAndValue(() => source.First());
        AssertAreEqual(expected, actual);
        sourceNativeArr.Dispose();
    }

    [Test, Parallelizable, Timeout(5000)]
    public void BlinqShouldEqualLinqNativeSequenceFirstPredicate([ArrayValues] int[] sourceArr)
    {
        var sourceNativeArr = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var source = sourceNativeArr.ToValueSequence();
        var expected = ExceptionAndValue(() => Linq.First(source, EqualsZero.Invoke));
        var actual = ExceptionAndValue(() => source.First(EqualsZero));
        AssertAreEqual(expected, actual);
        sourceNativeArr.Dispose();
    }

    [Test, Parallelizable, Timeout(5000)]
    public void BlinqShouldEqualLinqNativeSequenceRunFirst([ArrayValues] int[] sourceArr)
    {
        using (var sourceNativeArr = new NativeArray<int>(sourceArr, Allocator.Persistent))
        {
            var source = sourceNativeArr.ToValueSequence();
            var expected = ExceptionAndValue(() => Linq.First(source));
            if (expected.exception != null)
                return;
            var actual = ExceptionAndValue(() => source.RunFirst());
            AssertAreEqual(expected, actual);
        }
    }

    [Test, Parallelizable, Timeout(5000)]
    public void BlinqShouldEqualLinqNativeSequenceRunFirstPredicate([ArrayValues] int[] sourceArr)
    {
        using (var sourceNativeArr = new NativeArray<int>(sourceArr, Allocator.Persistent))
        {
            var source = sourceNativeArr.ToValueSequence();
            var expected = ExceptionAndValue(() => Linq.First(source, EqualsZero.Invoke));
            if (expected.exception != null)
                return;
            var actual = ExceptionAndValue(() => source.RunFirst(EqualsZero));
            AssertAreEqual(expected, actual);
        }
    }

    [Test, Parallelizable, Timeout(5000)]
    public void BlinqShouldEqualLinqNativeSequenceScheduleFirst([ArrayValues] int[] sourceArr)
    {
        using (var sourceNativeArr = new NativeArray<int>(sourceArr, Allocator.Persistent))
        {
            var source = sourceNativeArr.ToValueSequence();
            var expected = ExceptionAndValue(() => Linq.First(source));
            if (expected.exception != null)
                return;
            var actual = ExceptionAndValue(() => source.ScheduleFirst().Complete());
            AssertAreEqual(expected, actual);
        }
    }

    [Test, Parallelizable, Timeout(5000)]
    public void BlinqShouldEqualLinqNativeSequenceScheduleFirstPredicate([ArrayValues] int[] sourceArr)
    {
        using (var sourceNativeArr = new NativeArray<int>(sourceArr, Allocator.Persistent))
        {
            var source = sourceNativeArr.ToValueSequence();
            var expected = ExceptionAndValue(() => Linq.First(source, EqualsZero.Invoke));
            if (expected.exception != null)
                return;
            var actual = ExceptionAndValue(() => source.ScheduleFirst(EqualsZero).Complete());
            AssertAreEqual(expected, actual);
        }
    }

    [Test, Parallelizable, Timeout(5000)]
    public void BlinqShouldEqualLinqNativeArrayFirstOrDefault([ArrayValues] int[] sourceArr)
    {
        var source = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var expected = ExceptionAndValue(() => Linq.FirstOrDefault(source));
        var actual = ExceptionAndValue(() => source.FirstOrDefault());
        AssertAreEqual(expected, actual);
        source.Dispose();
    }

    [Test, Parallelizable, Timeout(5000)]
    public void BlinqShouldEqualLinqNativeArrayFirstOrDefaultPredicate([ArrayValues] int[] sourceArr)
    {
        var source = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var expected = ExceptionAndValue(() => Linq.FirstOrDefault(source, EqualsZero.Invoke));
        var actual = ExceptionAndValue(() => source.FirstOrDefault(EqualsZero));
        AssertAreEqual(expected, actual);
        source.Dispose();
    }

    [Test, Parallelizable, Timeout(5000)]
    public void BlinqShouldEqualLinqNativeSequenceFirstOrDefault([ArrayValues] int[] sourceArr)
    {
        var sourceNativeArr = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var source = sourceNativeArr.ToValueSequence();
        var expected = ExceptionAndValue(() => Linq.FirstOrDefault(source));
        var actual = ExceptionAndValue(() => source.FirstOrDefault());
        AssertAreEqual(expected, actual);
        sourceNativeArr.Dispose();
    }

    [Test, Parallelizable, Timeout(5000)]
    public void BlinqShouldEqualLinqNativeSequenceFirstOrDefaultPredicate([ArrayValues] int[] sourceArr)
    {
        var sourceNativeArr = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var source = sourceNativeArr.ToValueSequence();
        var expected = ExceptionAndValue(() => Linq.FirstOrDefault(source, EqualsZero.Invoke));
        var actual = ExceptionAndValue(() => source.FirstOrDefault(EqualsZero));
        AssertAreEqual(expected, actual);
        sourceNativeArr.Dispose();
    }

    [Test, Parallelizable, Timeout(5000)]
    public void BlinqShouldEqualLinqNativeSequenceRunFirstOrDefault([ArrayValues] int[] sourceArr)
    {
        using (var sourceNativeArr = new NativeArray<int>(sourceArr, Allocator.Persistent))
        {
            var source = sourceNativeArr.ToValueSequence();
            var expected = ExceptionAndValue(() => Linq.First(source));
            if (expected.exception != null)
                return;
            var actual = ExceptionAndValue(() => source.RunFirstOrDefault());
            AssertAreEqual(expected, actual);
        }
    }

    [Test, Parallelizable, Timeout(5000)]
    public void BlinqShouldEqualLinqNativeSequenceRunFirstOrDefaultPredicate([ArrayValues] int[] sourceArr)
    {
        using (var sourceNativeArr = new NativeArray<int>(sourceArr, Allocator.Persistent))
        {
            var source = sourceNativeArr.ToValueSequence();
            var expected = ExceptionAndValue(() => Linq.First(source, EqualsZero.Invoke));
            if (expected.exception != null)
                return;
            var actual = ExceptionAndValue(() => source.RunFirstOrDefault(EqualsZero));
            AssertAreEqual(expected, actual);
        }
    }

    [Test, Parallelizable, Timeout(5000)]
    public void BlinqShouldEqualLinqNativeSequenceScheduleFirstOrDefault([ArrayValues] int[] sourceArr)
    {
        using (var sourceNativeArr = new NativeArray<int>(sourceArr, Allocator.Persistent))
        {
            var source = sourceNativeArr.ToValueSequence();
            var expected = ExceptionAndValue(() => Linq.First(source));
            if (expected.exception != null)
                return;
            var actual = ExceptionAndValue(() => source.ScheduleFirstOrDefault().Complete());
            AssertAreEqual(expected, actual);
        }
    }

    [Test, Parallelizable, Timeout(5000)]
    public void BlinqShouldEqualLinqNativeSequenceScheduleFirstOrDefaultPredicate([ArrayValues] int[] sourceArr)
    {
        using (var sourceNativeArr = new NativeArray<int>(sourceArr, Allocator.Persistent))
        {
            var source = sourceNativeArr.ToValueSequence();
            var expected = ExceptionAndValue(() => Linq.First(source, EqualsZero.Invoke));
            if (expected.exception != null)
                return;
            var actual = ExceptionAndValue(() => source.ScheduleFirstOrDefault(EqualsZero).Complete());
            AssertAreEqual(expected, actual);
        }
    }
}
