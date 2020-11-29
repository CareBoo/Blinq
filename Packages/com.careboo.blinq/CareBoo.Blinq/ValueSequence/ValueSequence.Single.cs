﻿using System;
using Unity.Collections;
using CareBoo.Burst.Delegates;
using System.Collections.Generic;
using Unity.Jobs;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {

        public static T Single<T, TSource, TSourceEnumerator, TPredicate>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : unmanaged, IEquatable<T>
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var list = source.ToNativeList(Allocator.Temp);
            var set = new NativeHashSet<T>(list.Length, Allocator.Temp);
            for (var i = 0; i < list.Length; i++)
            {
                var val = list[i];
                if (predicate.Invoke(val) && !set.Add(val))
                {
                    set.Dispose();
                    list.Dispose();
                    throw Error.MoreThanOneMatch();
                }
            }
            var iter = set.GetEnumerator();
            if (iter.MoveNext())
            {
                var result = iter.Current;
                set.Dispose();
                list.Dispose();
                return result;
            }
            set.Dispose();
            list.Dispose();
            throw Error.NoMatch();
        }

        public struct SingleFunc<T, TSource, TSourceEnumerator, TPredicate>
            : IFunc<ValueSequence<T, TSource, TSourceEnumerator>, T>
            where T : unmanaged, IEquatable<T>
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            public ValueFunc<T, bool>.Struct<TPredicate> Predicate;
            public T Invoke(ValueSequence<T, TSource, TSourceEnumerator> arg0)
            {
                return arg0.Single(Predicate);
            }
        }

        public static ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, T>.Struct<SingleFunc<T, TSource, TSourceEnumerator, TPredicate>> NewSingleFunc<T, TSource, TSourceEnumerator, TPredicate>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : unmanaged, IEquatable<T>
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var func = new SingleFunc<T, TSource, TSourceEnumerator, TPredicate> { Predicate = predicate };
            return ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, T>.New(func);
        }

        public static T RunSingle<T, TSource, TSourceEnumerator, TPredicate>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : unmanaged, IEquatable<T>
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var singleFunc = source.NewSingleFunc(predicate);
            return source.Run(source.NewSingleFunc(predicate));
        }

        public static JobHandle ScheduleSingle<T, TSource, TSourceEnumerator, TPredicate>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ref NativeArray<T> output,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : unmanaged, IEquatable<T>
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var singleFunc = source.NewSingleFunc(predicate);
            return source.Schedule(source.NewSingleFunc(predicate), ref output);
        }

        public static JobHandle<T> ScheduleSingle<T, TSource, TSourceEnumerator, TPredicate>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : unmanaged, IEquatable<T>
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var singleFunc = source.NewSingleFunc(predicate);
            return source.Schedule(source.NewSingleFunc(predicate));
        }

        public static T SingleOrDefault<T, TSource, TSourceEnumerator, TPredicate>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate,
            in T defaultVal = default
            )
            where T : unmanaged, IEquatable<T>
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var list = source.ToNativeList(Allocator.Temp);
            var set = new NativeHashSet<T>(list.Length, Allocator.Temp);
            for (var i = 0; i < list.Length; i++)
            {
                var val = list[i];
                if (predicate.Invoke(val) && !set.Add(val))
                {
                    set.Dispose();
                    list.Dispose();
                    throw Error.MoreThanOneMatch();
                }
            }
            var iter = set.GetEnumerator();
            var result = iter.MoveNext()
                ? iter.Current
                : defaultVal;
            set.Dispose();
            list.Dispose();
            return result;
        }

        public struct SingleOrDefaultFunc<T, TSource, TSourceEnumerator, TPredicate>
            : IFunc<ValueSequence<T, TSource, TSourceEnumerator>, T>
            where T : unmanaged, IEquatable<T>
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            public ValueFunc<T, bool>.Struct<TPredicate> Predicate;
            public T Default;
            public T Invoke(ValueSequence<T, TSource, TSourceEnumerator> arg0)
            {
                return arg0.SingleOrDefault(Predicate, in Default);
            }
        }

        public static ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, T>.Struct<SingleOrDefaultFunc<T, TSource, TSourceEnumerator, TPredicate>> NewSingleOrDefaultFunc<T, TSource, TSourceEnumerator, TPredicate>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate,
            in T defaultVal = default
            )
            where T : unmanaged, IEquatable<T>
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var func = new SingleOrDefaultFunc<T, TSource, TSourceEnumerator, TPredicate> { Predicate = predicate, Default = defaultVal };
            return ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, T>.New(func);
        }

        public static T RunSingleOrDefault<T, TSource, TSourceEnumerator, TPredicate>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate,
            in T defaultVal = default
            )
            where T : unmanaged, IEquatable<T>
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var func = source.NewSingleOrDefaultFunc(predicate, in defaultVal);
            return source.Run(func);
        }

        public static JobHandle ScheduleSingleOrDefault<T, TSource, TSourceEnumerator, TPredicate>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ref NativeArray<T> output,
            ValueFunc<T, bool>.Struct<TPredicate> predicate,
            in T defaultVal = default
            )
            where T : unmanaged, IEquatable<T>
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var func = source.NewSingleOrDefaultFunc(predicate, in defaultVal);
            return source.Schedule(func, ref output);
        }

        public static JobHandle<T> ScheduleSingleOrDefault<T, TSource, TSourceEnumerator, TPredicate>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate,
            in T defaultVal = default
            )
            where T : unmanaged, IEquatable<T>
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var func = source.NewSingleOrDefaultFunc(predicate, in defaultVal);
            return source.Schedule(func);
        }

        public static T Single<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var list = source.ToNativeList(Allocator.Temp);
            if (list.Length > 1)
            {
                list.Dispose();
                throw Error.MoreThanOneElement();
            }
            if (list.Length == 0)
            {
                list.Dispose();
                throw Error.NoElements();
            }
            var result = list[0];
            list.Dispose();
            return result;
        }

        public struct SingleFunc<T, TSource, TSourceEnumerator>
            : IFunc<ValueSequence<T, TSource, TSourceEnumerator>, T>
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            public T Invoke(ValueSequence<T, TSource, TSourceEnumerator> arg0)
            {
                return arg0.Single();
            }
        }

        public static ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, T>.Struct<SingleFunc<T, TSource, TSourceEnumerator>> NewSingleFunc<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            return default(ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, T>.Struct<SingleFunc<T, TSource, TSourceEnumerator>>);
        }

        public static T RunSingle<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var func = source.NewSingleFunc();
            return source.Run(func);
        }

        public static JobHandle ScheduleSingle<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ref NativeArray<T> output
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var func = source.NewSingleFunc();
            return source.Schedule(func, ref output);
        }

        public static JobHandle<T> ScheduleSingle<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var func = source.NewSingleFunc();
            return source.Schedule(func);
        }

        public static T SingleOrDefault<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            in T defaultVal = default
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var list = source.ToNativeList(Allocator.Temp);
            if (list.Length > 1)
            {
                list.Dispose();
                throw Error.MoreThanOneElement();
            }
            var result = list.Length == 0
                ? defaultVal
                : list[0];
            list.Dispose();
            return result;
        }

        public struct SingleOrDefaultFunc<T, TSource, TSourceEnumerator>
            : IFunc<ValueSequence<T, TSource, TSourceEnumerator>, T>
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            public T Default;
            public T Invoke(ValueSequence<T, TSource, TSourceEnumerator> arg0)
            {
                return arg0.SingleOrDefault(in Default);
            }
        }

        public static ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, T>.Struct<SingleOrDefaultFunc<T, TSource, TSourceEnumerator>> NewSingleOrDefaultFunc<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            in T defaultVal = default
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var func = new SingleOrDefaultFunc<T, TSource, TSourceEnumerator> { Default = defaultVal };
            return ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, T>.New(func);
        }

        public static T RunSingleOrDefault<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            in T defaultVal = default
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var func = source.NewSingleOrDefaultFunc(in defaultVal);
            return source.Run(func);
        }

        public static JobHandle ScheduleSingleOrDefault<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ref NativeArray<T> output,
            in T defaultVal = default
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var func = source.NewSingleOrDefaultFunc(in defaultVal);
            return source.Schedule(func, ref output);
        }

        public static JobHandle<T> ScheduleSingleOrDefault<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            in T defaultVal = default
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var func = source.NewSingleOrDefaultFunc(in defaultVal);
            return source.Schedule(func);
        }
    }
}
