using NUnit.Framework;
using Unity.PerformanceTesting;
using Linq = System.Linq.Enumerable;
using Blinq = CareBoo.Blinq.NativeArrayExtensions;

internal class SelectTest : BaseBlinqPerformanceTest
{
    [Test, Performance]
    [Category("Performance")]
    public void BlinqSelectNativeSequencePerformance()
    {
        MeasureBlinq(() => Blinq.Select<int, long, IntToLong>(ref source)).Run();
    }

    [Test, Performance]
    [Category("Performance")]
    public void LinqSelectNativeSequencePerformance()
    {
        MeasureLinq(() => Linq.Select(source, default(IntToLong).Invoke)).Run();
    }
}
