namespace CareBoo.Blinq.Tests
{
    public struct EqualsZero : IPredicate<int>
    {
        public bool Invoke(int val)
        {
            return val == 0;
        }
    }
}
