namespace CareBoo.Blinq
{

    public interface IAccumulator<TSource, TAccumulate>
        where TSource : unmanaged
        where TAccumulate : unmanaged
    {
        TAccumulate Invoke(TSource source, TAccumulate seed);
    }

    public interface IAccumulator<TSource> : IAccumulator<TSource, TSource>
        where TSource : unmanaged
    {
    }
}