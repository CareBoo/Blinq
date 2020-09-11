using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class NativeArrayExtensions
    {
        public static bool All<T>(this ref NativeArray<T> source, ValueFunc<T, bool> predicate)
            where T : struct
        {
            for (var i = 0; i < source.Length; i++)
                if (!predicate.Invoke(source[i]))
                    return false;
            return true;
        }
    }
}
