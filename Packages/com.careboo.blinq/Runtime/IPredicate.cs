namespace CareBoo.Blinq
{
    public interface IPredicate<T> : IFunc<T, bool>
        where T : unmanaged
    { }
}
