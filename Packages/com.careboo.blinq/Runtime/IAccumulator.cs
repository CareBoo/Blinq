namespace CareBoo.Blinq
{

    public interface IAccumulator<TSource, TAccumulate> : IFunc<TSource, TAccumulate, TAccumulate>
        where TSource : unmanaged
        where TAccumulate : unmanaged
    {
    }

    public interface IAccumulator<TSource> : IAccumulator<TSource, TSource>
        where TSource : unmanaged
    {
    }
}