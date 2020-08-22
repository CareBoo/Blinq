namespace CareBoo.Blinq
{
    public interface IPredicate<T> where T : unmanaged
    {
        bool Invoke(T value);
    }
}
