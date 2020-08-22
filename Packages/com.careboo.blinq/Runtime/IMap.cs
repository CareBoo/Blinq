namespace CareBoo.Blinq
{
    public interface IMap<TSource, TResult>
        where TSource : unmanaged
        where TResult : unmanaged
    {
        TResult Invoke(TSource source);
    }
}