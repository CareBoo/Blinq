namespace CareBoo.Blinq
{

    public interface IAccumulator<TSource, TAccumulate> : IFunc<TSource, TAccumulate, TAccumulate>
        where TSource : struct
        where TAccumulate : struct
    {
    }

    public interface IAccumulator<TSource> : IAccumulator<TSource, TSource>
        where TSource : struct
    {
    }
}