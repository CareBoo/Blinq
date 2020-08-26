namespace CareBoo.Blinq.Tests
{
    public struct EqualsZero : IFunc<int, bool>, IFunc<int, int, bool>
    {
        public bool Invoke(int val)
        {
            return val == 0;
        }

        public bool Invoke(int val, int index)
        {
            return val == 0;
        }
    }
}
