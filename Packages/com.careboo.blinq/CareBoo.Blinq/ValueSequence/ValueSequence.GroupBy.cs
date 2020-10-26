using System;
using Unity.Collections;
using CareBoo.Burst.Delegates;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<TResult, GroupBySequence<T, TSource, TKey, TKeySelector, TElement, TElementSelector, TResult, TResultSelector>> GroupBy<T, TSource, TKey, TKeySelector, TElement, TElementSelector, TResult, TResultSelector>(
            this ValueSequence<T, TSource> source,
            ValueFunc<T, TKey>.Struct<TKeySelector> keySelector,
            ValueFunc<T, TElement>.Struct<TElementSelector> elementSelector,
            ValueFunc<TKey, NativeMultiHashMap<TKey, TElement>, TResult>.Struct<TResultSelector> resultSelector
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TKey : unmanaged, IEquatable<TKey>
            where TKeySelector : struct, IFunc<T, TKey>
            where TElement : struct
            where TElementSelector : struct, IFunc<T, TElement>
            where TResult : struct
            where TResultSelector : struct, IFunc<TKey, NativeMultiHashMap<TKey, TElement>, TResult>
        {
            var seq = GroupBySequence.New(source.Source, keySelector, elementSelector, resultSelector);
            return ValueSequence<TResult>.New(seq);
        }

        public static ValueSequence<TResult, GroupBySequence<T, TSource, TKey, TKeySelector, T, SameSelector<T>, TResult, TResultSelector>> GroupBy<T, TSource, TKey, TKeySelector, TResult, TResultSelector>(
            this ValueSequence<T, TSource> source,
            ValueFunc<T, TKey>.Struct<TKeySelector> keySelector,
            ValueFunc<TKey, NativeMultiHashMap<TKey, T>, TResult>.Struct<TResultSelector> resultSelector
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TKey : unmanaged, IEquatable<TKey>
            where TKeySelector : struct, IFunc<T, TKey>
            where TResult : struct
            where TResultSelector : struct, IFunc<TKey, NativeMultiHashMap<TKey, T>, TResult>
        {
            var elementSelector = UtilFunctions.SameSelector<T>();
            return source.GroupBy(keySelector, elementSelector, resultSelector);
        }
    }

    public struct GroupBySequence<T, TSource, TKey, TKeySelector, TElement, TElementSelector, TResult, TResultSelector>
        : ISequence<TResult>
        where T : struct
        where TSource : struct, ISequence<T>
        where TKey : unmanaged, IEquatable<TKey>
        where TKeySelector : struct, IFunc<T, TKey>
        where TElement : struct
        where TElementSelector : struct, IFunc<T, TElement>
        where TResult : struct
        where TResultSelector : struct, IFunc<TKey, NativeMultiHashMap<TKey, TElement>, TResult>
    {
        readonly TSource source;
        readonly ValueFunc<T, TKey>.Struct<TKeySelector> keySelector;
        readonly ValueFunc<T, TElement>.Struct<TElementSelector> elementSelector;
        readonly ValueFunc<TKey, NativeMultiHashMap<TKey, TElement>, TResult>.Struct<TResultSelector> resultSelector;

        public GroupBySequence(
            TSource source,
            ValueFunc<T, TKey>.Struct<TKeySelector> keySelector,
            ValueFunc<T, TElement>.Struct<TElementSelector> elementSelector,
            ValueFunc<TKey, NativeMultiHashMap<TKey, TElement>, TResult>.Struct<TResultSelector> resultSelector
            )
        {
            this.source = source;
            this.keySelector = keySelector;
            this.elementSelector = elementSelector;
            this.resultSelector = resultSelector;
        }

        public NativeList<TResult> ToList()
        {
            using (var srcList = source.ToList())
            using (var groupMap = new NativeMultiHashMap<TKey, TElement>(srcList.Length, Allocator.Temp))
            {
                return Execute(srcList, groupMap);
            }
        }

        public NativeList<TResult> Execute(NativeList<T> srcList, NativeMultiHashMap<TKey, TElement> groupMap)
        {
            var results = new NativeList<TResult>(Allocator.Temp);
            AddElementsToGroupMap(srcList, groupMap);
            EnumerateGroupsAndAddToResults(groupMap, results);
            return results;
        }

        private void AddElementsToGroupMap(NativeList<T> srcList, NativeMultiHashMap<TKey, TElement> groupMap)
        {
            for (var i = 0; i < srcList.Length; i++)
            {
                var item = srcList[i];
                var key = keySelector.Invoke(item);
                var element = elementSelector.Invoke(item);
                groupMap.Add(key, element);
            }
        }

        private void EnumerateGroupsAndAddToResults(NativeMultiHashMap<TKey, TElement> groupMap, NativeList<TResult> results)
        {
            using (var keysArr = groupMap.GetKeyArray(Allocator.Temp))
            using (var keySet = new NativeHashSet<TKey>(keysArr.Length, Allocator.Temp))
            {
                for (var i = 0; i < keysArr.Length; i++)
                {
                    var key = keysArr[i];
                    if (keySet.Add(key))
                    {
                        var result = resultSelector.Invoke(key, groupMap);
                        results.Add(result);
                    }
                }
            }
        }
    }

    public static class GroupBySequence
    {
        public static GroupBySequence<T, TSource, TKey, TKeySelector, TElement, TElementSelector, TResult, TResultSelector> New<T, TSource, TKey, TKeySelector, TElement, TElementSelector, TResult, TResultSelector>(
            TSource source,
            ValueFunc<T, TKey>.Struct<TKeySelector> keySelector,
            ValueFunc<T, TElement>.Struct<TElementSelector> elementSelector,
            ValueFunc<TKey, NativeMultiHashMap<TKey, TElement>, TResult>.Struct<TResultSelector> resultSelector
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TKey : unmanaged, IEquatable<TKey>
            where TKeySelector : struct, IFunc<T, TKey>
            where TElement : struct
            where TElementSelector : struct, IFunc<T, TElement>
            where TResult : struct
            where TResultSelector : struct, IFunc<TKey, NativeMultiHashMap<TKey, TElement>, TResult>
        {
            return new GroupBySequence<T, TSource, TKey, TKeySelector, TElement, TElementSelector, TResult, TResultSelector>(source, keySelector, elementSelector, resultSelector);
        }
    }
}
