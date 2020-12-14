using System;
using System.Collections.Generic;
using CareBoo.Burst.Delegates;
using Unity.Collections;
using Unity.Jobs;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static bool Contains<T>(
            this in NativeArray<T> source,
            in T item
            )
            where T : struct, IEquatable<T>
        {
            for (var i = 0; i < source.Length; i++)
                if (source[i].Equals(item))
                    return true;
            return false;
        }

        public struct ArrayContainsFunc<T>
            : IFunc<NativeArray<T>, bool>
            where T : struct, IEquatable<T>
        {
            public T Item;

            public bool Invoke(NativeArray<T> arr)
            {
                return arr.Contains(Item);
            }
        }

        public static ValueFunc<NativeArray<T>, bool>.Struct<ArrayContainsFunc<T>>
        ContainsAsFunc<T>(
            this in NativeArray<T> source,
            in T item
            )
            where T : struct, IEquatable<T>
        {
            var funcStruct = new ArrayContainsFunc<T> { Item = item };
            return ValueFunc<NativeArray<T>, bool>.New(funcStruct);
        }

        public static bool RunContains<T>(
            this in NativeArray<T> source,
            in T item
            )
            where T : struct, IEquatable<T>
        {
            var func = source.ContainsAsFunc(item);
            return source.Run(func);
        }

        public static JobHandle<bool> ScheduleContains<T>(
            this in NativeArray<T> source,
            in T item
            )
            where T : struct, IEquatable<T>
        {
            var func = source.ContainsAsFunc(item);
            return source.Schedule(func);
        }

        public static JobHandle ScheduleContains<T>(
            this in NativeArray<T> source,
            ref NativeArray<bool> output,
            in T item
            )
            where T : struct, IEquatable<T>
        {
            var func = source.ContainsAsFunc(item);
            return source.Schedule(func, ref output);
        }

        public static bool Contains<T, TComparer>(
            this in NativeArray<T> source,
            in T item,
            in TComparer comparer
            )
            where T : struct
            where TComparer : struct, IEqualityComparer<T>
        {
            for (var i = 0; i < source.Length; i++)
                if (comparer.Equals(source[i], item))
                    return true;
            return false;
        }

        public struct ArrayContainsFunc<T, TComparer>
            : IFunc<NativeArray<T>, bool>
            where T : struct
            where TComparer : struct, IEqualityComparer<T>
        {
            public T Item;
            public TComparer Comparer;

            public bool Invoke(NativeArray<T> arr)
            {
                return arr.Contains(Item, Comparer);
            }
        }

        public static ValueFunc<NativeArray<T>, bool>.Struct<ArrayContainsFunc<T, TComparer>>
        ContainsAsFunc<T, TComparer>(
            this in NativeArray<T> source,
            in T item,
            in TComparer comparer
            )
            where T : struct
            where TComparer : struct, IEqualityComparer<T>
        {
            var funcStruct = new ArrayContainsFunc<T, TComparer> { Item = item, Comparer = comparer };
            return ValueFunc<NativeArray<T>, bool>.New(funcStruct);
        }

        public static bool RunContains<T, TComparer>(
            this in NativeArray<T> source,
            in T item,
            in TComparer comparer
            )
            where T : struct
            where TComparer : struct, IEqualityComparer<T>
        {
            var func = source.ContainsAsFunc(item, comparer);
            return source.Run(func);
        }

        public static JobHandle<bool> ScheduleContains<T, TComparer>(
            this in NativeArray<T> source,
            in T item,
            in TComparer comparer
            )
            where T : struct
            where TComparer : struct, IEqualityComparer<T>
        {
            var func = source.ContainsAsFunc(item, comparer);
            return source.Schedule(func);
        }

        public static JobHandle ScheduleContains<T, TComparer>(
            this in NativeArray<T> source,
            ref NativeArray<bool> output,
            in T item,
            in TComparer comparer
            )
            where T : struct
            where TComparer : struct, IEqualityComparer<T>
        {
            var func = source.ContainsAsFunc(item, comparer);
            return source.Schedule(func, ref output);
        }
    }
}
