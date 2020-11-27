using System;
using Unity.Collections;
using CareBoo.Burst.Delegates;
using System.Collections;
using Unity.Collections.LowLevel.Unsafe;
using System.Collections.Generic;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<
            TResult,
            GroupBySequence<T, TSource, TKey, TKeySelector, TElement, TElementSelector, TResult, TResultSelector>,
            SequenceEnumerator<TResult, GroupBySequence<T, TSource, TKey, TKeySelector, TElement, TElementSelector, TResult, TResultSelector>>>
        GroupBy<T, TSource, TSourceEnumerator, TKey, TKeySelector, TElement, TElementSelector, TResult, TResultSelector>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, TKey>.Struct<TKeySelector> keySelector,
            ValueFunc<T, TElement>.Struct<TElementSelector> elementSelector,
            ValueFunc<TKey, ValueGroupingValues<TKey, TElement>, TResult>.Struct<TResultSelector> resultSelector
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TKey : unmanaged, IEquatable<TKey>
            where TKeySelector : struct, IFunc<T, TKey>
            where TElement : struct
            where TElementSelector : struct, IFunc<T, TElement>
            where TResult : struct
            where TResultSelector : struct, IFunc<TKey, ValueGroupingValues<TKey, TElement>, TResult>
        {
            var sourceSeq = source.Source;
            var seq = GroupBySequence.New(in sourceSeq, keySelector, elementSelector, resultSelector);
            return ValueSequence<TResult, SequenceEnumerator<TResult, GroupBySequence<T, TSource, TKey, TKeySelector, TElement, TElementSelector, TResult, TResultSelector>>>.New(in seq);
        }

        public static ValueSequence<
            TResult,
            GroupBySequence<T, TSource, TKey, TKeySelector, T, SameSelector<T>, TResult, TResultSelector>,
            SequenceEnumerator<TResult, GroupBySequence<T, TSource, TKey, TKeySelector, T, SameSelector<T>, TResult, TResultSelector>>>
        GroupBy<T, TSource, TSourceEnumerator, TKey, TKeySelector, TResult, TResultSelector>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, TKey>.Struct<TKeySelector> keySelector,
            ValueFunc<TKey, ValueGroupingValues<TKey, T>, TResult>.Struct<TResultSelector> resultSelector
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TKey : unmanaged, IEquatable<TKey>
            where TKeySelector : struct, IFunc<T, TKey>
            where TResult : struct
            where TResultSelector : struct, IFunc<TKey, ValueGroupingValues<TKey, T>, TResult>
        {
            var elementSelector = UtilFunctions.SameSelector<T>();
            return source.GroupBy(keySelector, elementSelector, resultSelector);
        }

        public static ValueSequence<
            ValueGrouping<TKey, TElement>,
            GroupBySequence<T, TSource, TKey, TKeySelector, TElement, TElementSelector, ValueGrouping<TKey, TElement>, GroupSelector<TKey, TElement>>,
            SequenceEnumerator<ValueGrouping<TKey, TElement>, GroupBySequence<T, TSource, TKey, TKeySelector, TElement, TElementSelector, ValueGrouping<TKey, TElement>, GroupSelector<TKey, TElement>>>>
        GroupBy<T, TSource, TSourceEnumerator, TKey, TKeySelector, TElement, TElementSelector>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, TKey>.Struct<TKeySelector> keySelector,
            ValueFunc<T, TElement>.Struct<TElementSelector> elementSelector
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TKey : unmanaged, IEquatable<TKey>
            where TKeySelector : struct, IFunc<T, TKey>
            where TElement : struct
            where TElementSelector : struct, IFunc<T, TElement>
        {
            var resultSelector = UtilFunctions.GroupSelector<TKey, TElement>();
            return source.GroupBy(keySelector, elementSelector, resultSelector);
        }

        public static ValueSequence<
            ValueGrouping<TKey, T>,
            GroupBySequence<T, TSource, TKey, TKeySelector, T, SameSelector<T>, ValueGrouping<TKey, T>, GroupSelector<TKey, T>>,
            SequenceEnumerator<ValueGrouping<TKey, T>, GroupBySequence<T, TSource, TKey, TKeySelector, T, SameSelector<T>, ValueGrouping<TKey, T>, GroupSelector<TKey, T>>>>
        GroupBy<T, TSource, TSourceEnumerator, TKey, TKeySelector>(
            this ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, TKey>.Struct<TKeySelector> keySelector
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TKey : unmanaged, IEquatable<TKey>
            where TKeySelector : struct, IFunc<T, TKey>
        {
            var elementSelector = UtilFunctions.SameSelector<T>();
            var resultSelector = UtilFunctions.GroupSelector<TKey, T>();
            return source.GroupBy(keySelector, elementSelector, resultSelector);
        }
    }

    public struct GroupBySequence<T, TSource, TKey, TKeySelector, TElement, TElementSelector, TResult, TResultSelector>
        : ISequence<TResult, SequenceEnumerator<TResult, GroupBySequence<T, TSource, TKey, TKeySelector, TElement, TElementSelector, TResult, TResultSelector>>>
        where T : struct
        where TSource : struct, INativeListConvertible<T>
        where TKey : unmanaged, IEquatable<TKey>
        where TKeySelector : struct, IFunc<T, TKey>
        where TElement : struct
        where TElementSelector : struct, IFunc<T, TElement>
        where TResult : struct
        where TResultSelector : struct, IFunc<TKey, ValueGroupingValues<TKey, TElement>, TResult>
    {
        readonly TSource source;
        readonly ValueFunc<T, TKey>.Struct<TKeySelector> keySelector;
        readonly ValueFunc<T, TElement>.Struct<TElementSelector> elementSelector;
        readonly ValueFunc<TKey, ValueGroupingValues<TKey, TElement>, TResult>.Struct<TResultSelector> resultSelector;

        public GroupBySequence(
            in TSource source,
            ValueFunc<T, TKey>.Struct<TKeySelector> keySelector,
            ValueFunc<T, TElement>.Struct<TElementSelector> elementSelector,
            ValueFunc<TKey, ValueGroupingValues<TKey, TElement>, TResult>.Struct<TResultSelector> resultSelector
            )
        {
            this.source = source;
            this.keySelector = keySelector;
            this.elementSelector = elementSelector;
            this.resultSelector = resultSelector;
        }

        public SequenceEnumerator<TResult, GroupBySequence<T, TSource, TKey, TKeySelector, TElement, TElementSelector, TResult, TResultSelector>> GetEnumerator()
        {
            return SequenceEnumerator<TResult>.New(in this);
        }

        public NativeList<TResult> ToNativeList(Allocator allocator)
        {
            var srcList = source.ToNativeList(Allocator.Temp);
            var groupMap = new UnsafeMultiHashMap<TKey, TElement>(srcList.Length, Allocator.Temp);
            var result = ToNativeList(srcList, groupMap, allocator);
            srcList.Dispose();
            groupMap.Dispose();
            return result;
        }

        public NativeList<TResult> ToNativeList(NativeList<T> srcList, UnsafeMultiHashMap<TKey, TElement> groupMap, Allocator allocator)
        {
            var results = new NativeList<TResult>(allocator);
            AddElementsToGroupMap(in srcList, ref groupMap);
            EnumerateGroupsAndAddToResults(in groupMap, ref results);
            return results;
        }

        private void AddElementsToGroupMap(in NativeList<T> srcList, ref UnsafeMultiHashMap<TKey, TElement> groupMap)
        {
            for (var i = 0; i < srcList.Length; i++)
            {
                var item = srcList[i];
                var key = keySelector.Invoke(item);
                var element = elementSelector.Invoke(item);
                groupMap.Add(key, element);
            }
        }

        private void EnumerateGroupsAndAddToResults(in UnsafeMultiHashMap<TKey, TElement> groupMap, ref NativeList<TResult> results)
        {
            var keysArr = groupMap.GetKeyArray(Allocator.Temp);
            var keySet = new NativeHashSet<TKey>(keysArr.Length, Allocator.Temp);
            for (var i = 0; i < keysArr.Length; i++)
            {
                var key = keysArr[i];
                if (keySet.Add(key))
                {
                    var values = new ValueGroupingValues<TKey, TElement>(key, groupMap);
                    var result = resultSelector.Invoke(key, values);
                    results.Add(result);
                }
            }
            keysArr.Dispose();
            keySet.Dispose();
        }

        IEnumerator<TResult> IEnumerable<TResult>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public static class GroupBySequence
    {
        public static GroupBySequence<T, TSource, TKey, TKeySelector, TElement, TElementSelector, TResult, TResultSelector> New<T, TSource, TKey, TKeySelector, TElement, TElementSelector, TResult, TResultSelector>(
            in TSource source,
            ValueFunc<T, TKey>.Struct<TKeySelector> keySelector,
            ValueFunc<T, TElement>.Struct<TElementSelector> elementSelector,
            ValueFunc<TKey, ValueGroupingValues<TKey, TElement>, TResult>.Struct<TResultSelector> resultSelector
            )
            where T : struct
            where TSource : struct, INativeListConvertible<T>
            where TKey : unmanaged, IEquatable<TKey>
            where TKeySelector : struct, IFunc<T, TKey>
            where TElement : struct
            where TElementSelector : struct, IFunc<T, TElement>
            where TResult : struct
            where TResultSelector : struct, IFunc<TKey, ValueGroupingValues<TKey, TElement>, TResult>
        {
            return new GroupBySequence<T, TSource, TKey, TKeySelector, TElement, TElementSelector, TResult, TResultSelector>(in source, keySelector, elementSelector, resultSelector);
        }
    }
}
