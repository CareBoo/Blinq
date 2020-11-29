using System.Collections.Generic;
using CareBoo.Burst.Delegates;
using Unity.Collections;
using Unity.Jobs;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static T First<T, TSource, TSourceEnumerator, TPredicate>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var seq = source.GetEnumerator();
            while (seq.MoveNext())
            {
                var current = seq.Current;
                if (predicate.Invoke(current))
                {
                    seq.Dispose();
                    return current;
                }
            }
            seq.Dispose();
            throw Error.NoMatch();
        }

        public struct FirstFunc<T, TSource, TSourceEnumerator, TPredicate>
            : IFunc<ValueSequence<T, TSource, TSourceEnumerator>, T>
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            public ValueFunc<T, bool>.Struct<TPredicate> Predicate;
            public T Invoke(ValueSequence<T, TSource, TSourceEnumerator> arg0)
            {
                return arg0.First(Predicate);
            }
        }

        public static ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, T>.Struct<FirstFunc<T, TSource, TSourceEnumerator, TPredicate>> NewFirstFunc<T, TSource, TSourceEnumerator, TPredicate>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var func = new FirstFunc<T, TSource, TSourceEnumerator, TPredicate> { Predicate = predicate };
            return ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, T>.New(func);
        }

        public static T RunFirst<T, TSource, TSourceEnumerator, TPredicate>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var FirstFunc = source.NewFirstFunc(predicate);
            return source.Run(source.NewFirstFunc(predicate));
        }

        public static JobHandle ScheduleFirst<T, TSource, TSourceEnumerator, TPredicate>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ref NativeArray<T> output,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var FirstFunc = source.NewFirstFunc(predicate);
            return source.Schedule(source.NewFirstFunc(predicate), ref output);
        }

        public static JobHandle<T> ScheduleFirst<T, TSource, TSourceEnumerator, TPredicate>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var FirstFunc = source.NewFirstFunc(predicate);
            return source.Schedule(source.NewFirstFunc(predicate));
        }

        public static T FirstOrDefault<T, TSource, TSourceEnumerator, TPredicate>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate,
            in T defaultVal = default
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var seq = source.GetEnumerator();
            while (seq.MoveNext())
            {
                var current = seq.Current;
                if (predicate.Invoke(current))
                {
                    seq.Dispose();
                    return current;
                }
            }
            seq.Dispose();
            return defaultVal;
        }

        public struct FirstOrDefaultFunc<T, TSource, TSourceEnumerator, TPredicate>
            : IFunc<ValueSequence<T, TSource, TSourceEnumerator>, T>
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            public ValueFunc<T, bool>.Struct<TPredicate> Predicate;
            public T Default;
            public T Invoke(ValueSequence<T, TSource, TSourceEnumerator> arg0)
            {
                return arg0.FirstOrDefault(Predicate, in Default);
            }
        }

        public static ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, T>.Struct<FirstOrDefaultFunc<T, TSource, TSourceEnumerator, TPredicate>> NewFirstOrDefaultFunc<T, TSource, TSourceEnumerator, TPredicate>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate,
            in T defaultVal = default
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var func = new FirstOrDefaultFunc<T, TSource, TSourceEnumerator, TPredicate> { Predicate = predicate, Default = defaultVal };
            return ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, T>.New(func);
        }

        public static T RunFirstOrDefault<T, TSource, TSourceEnumerator, TPredicate>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate,
            in T defaultVal = default
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var func = source.NewFirstOrDefaultFunc(predicate, in defaultVal);
            return source.Run(func);
        }

        public static JobHandle ScheduleFirstOrDefault<T, TSource, TSourceEnumerator, TPredicate>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ref NativeArray<T> output,
            ValueFunc<T, bool>.Struct<TPredicate> predicate,
            in T defaultVal = default
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var func = source.NewFirstOrDefaultFunc(predicate, in defaultVal);
            return source.Schedule(func, ref output);
        }

        public static JobHandle<T> ScheduleFirstOrDefault<T, TSource, TSourceEnumerator, TPredicate>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, bool>.Struct<TPredicate> predicate,
            in T defaultVal = default
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TPredicate : struct, IFunc<T, bool>
        {
            var func = source.NewFirstOrDefaultFunc(predicate, in defaultVal);
            return source.Schedule(func);
        }


        public static T First<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var seq = source.GetEnumerator();
            if (!seq.MoveNext())
            {
                seq.Dispose();
                throw Error.NoElements();
            }
            var result = seq.Current;
            seq.Dispose();
            return result;
        }

        public struct FirstFunc<T, TSource, TSourceEnumerator>
            : IFunc<ValueSequence<T, TSource, TSourceEnumerator>, T>
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            public T Invoke(ValueSequence<T, TSource, TSourceEnumerator> arg0)
            {
                return arg0.First();
            }
        }

        public static ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, T>.Struct<FirstFunc<T, TSource, TSourceEnumerator>> NewFirstFunc<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            return default(ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, T>.Struct<FirstFunc<T, TSource, TSourceEnumerator>>);
        }

        public static T RunFirst<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var func = source.NewFirstFunc();
            return source.Run(func);
        }

        public static JobHandle ScheduleFirst<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ref NativeArray<T> output
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var func = source.NewFirstFunc();
            return source.Schedule(func, ref output);
        }

        public static JobHandle<T> ScheduleFirst<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var func = source.NewFirstFunc();
            return source.Schedule(func);
        }


        public static T FirstOrDefault<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            in T defaultVal = default
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var seq = source.GetEnumerator();
            var result = seq.MoveNext()
                ? seq.Current
                : defaultVal;
            seq.Dispose();
            return result;
        }

        public struct FirstOrDefaultFunc<T, TSource, TSourceEnumerator>
            : IFunc<ValueSequence<T, TSource, TSourceEnumerator>, T>
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            public T Default;
            public T Invoke(ValueSequence<T, TSource, TSourceEnumerator> arg0)
            {
                return arg0.FirstOrDefault(in Default);
            }
        }

        public static ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, T>.Struct<FirstOrDefaultFunc<T, TSource, TSourceEnumerator>> NewFirstOrDefaultFunc<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            in T defaultVal = default
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var func = new FirstOrDefaultFunc<T, TSource, TSourceEnumerator> { Default = defaultVal };
            return ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, T>.New(func);
        }

        public static T RunFirstOrDefault<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            in T defaultVal = default
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var func = source.NewFirstOrDefaultFunc(in defaultVal);
            return source.Run(func);
        }

        public static JobHandle ScheduleFirstOrDefault<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ref NativeArray<T> output,
            in T defaultVal = default
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var func = source.NewFirstOrDefaultFunc(in defaultVal);
            return source.Schedule(func, ref output);
        }

        public static JobHandle<T> ScheduleFirstOrDefault<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            in T defaultVal = default
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var func = source.NewFirstOrDefaultFunc(in defaultVal);
            return source.Schedule(func);
        }
    }
}
