﻿using NUnit.Framework;
using Linq = System.Linq.Enumerable;
using static Utils;
using Unity.Collections;
using Blinq = CareBoo.Blinq.NativeArrayExtensions;

internal class WhereTest
{
    [Test, Parallelizable]
    public void BlinqShouldEqualLinqNativeArrayWhereWithIndex([NativeArrayValues] NativeArray<int> source)
    {
        var (expectedException, expectedValue) = ExceptionOrValue(() => Linq.ToArray(Linq.Where(source, default(EqualToIndex).Invoke)));
        var (actualException, actualValue) = ExceptionOrValue(() => Linq.ToArray(Blinq.WhereWithIndex<int, EqualToIndex>(ref source)));
        Assert.AreEqual(expectedException, actualException);
        Assert.AreEqual(expectedValue, actualValue);
        source.Dispose();
    }

    [Test, Parallelizable]
    public void BlinqShouldEqualLinqNativeArrayWhere([NativeArrayValues] NativeArray<int> source)
    {
        var (expectedException, expectedValue) = ExceptionOrValue(() => Linq.ToArray(Linq.Where(source, default(EqualsZero).Invoke)));
        var (actualException, actualValue) = ExceptionOrValue(() => Linq.ToArray(Blinq.Where<int, EqualsZero>(ref source)));
        Assert.AreEqual(expectedException, actualException);
        Assert.AreEqual(expectedValue, actualValue);
        source.Dispose();
    }
}