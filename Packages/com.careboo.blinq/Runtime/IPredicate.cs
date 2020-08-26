namespace CareBoo.Blinq
{
    public interface IPredicate<T> : IFunc<T, bool>
        where T : struct
    {
    }

    public interface IPredicate<T, U> : IFunc<T, U, bool>
        where T : struct
        where U : struct
    {
    }
}
