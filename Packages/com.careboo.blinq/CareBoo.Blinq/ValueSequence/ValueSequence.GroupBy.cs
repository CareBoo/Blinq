﻿using System;
using Unity.Collections;
using CareBoo.Burst.Delegates;
using System.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<TResult, GroupBySequence<T, TSource, TKey, TKeySelector, TElement, TElementSelector, TResult, TResultSelector>> GroupBy<T, TSource, TKey, TKeySelector, TElement, TElementSelector, TResult, TResultSelector>(
            this in ValueSequence<T, TSource> source,
            in ValueFunc<T, TKey>.Struct<TKeySelector> keySelector,
            in ValueFunc<T, TElement>.Struct<TElementSelector> elementSelector,
            in ValueFunc<TKey, NativeMultiHashMap<TKey, TElement>, TResult>.Struct<TResultSelector> resultSelector
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
            var sourceSeq = source.GetEnumerator();
            var seq = GroupBySequence.New(ref sourceSeq, keySelector, elementSelector, resultSelector);
            return ValueSequence<TResult>.New(ref seq);
        }

        public static ValueSequence<TResult, GroupBySequence<T, TSource, TKey, TKeySelector, T, SameSelector<T>, TResult, TResultSelector>> GroupBy<T, TSource, TKey, TKeySelector, TResult, TResultSelector>(
            this in ValueSequence<T, TSource> source,
            in ValueFunc<T, TKey>.Struct<TKeySelector> keySelector,
            in ValueFunc<TKey, NativeMultiHashMap<TKey, T>, TResult>.Struct<TResultSelector> resultSelector
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
        readonly ValueFunc<T, TKey>.Struct<TKeySelector> keySelector;
        readonly ValueFunc<T, TElement>.Struct<TElementSelector> elementSelector;
        readonly ValueFunc<TKey, NativeMultiHashMap<TKey, TElement>, TResult>.Struct<TResultSelector> resultSelector;

        TSource source;
        NativeList<TResult> resultList;
        NativeArray<TResult>.Enumerator resultListEnum;

        public TResult Current => resultListEnum.Current;

        object IEnumerator.Current => Current;

        public GroupBySequence(
            ref TSource source,
            ValueFunc<T, TKey>.Struct<TKeySelector> keySelector,
            ValueFunc<T, TElement>.Struct<TElementSelector> elementSelector,
            ValueFunc<TKey, NativeMultiHashMap<TKey, TElement>, TResult>.Struct<TResultSelector> resultSelector
            )
        {
            this.source = source;
            this.keySelector = keySelector;
            this.elementSelector = elementSelector;
            this.resultSelector = resultSelector;
            resultList = default;
            resultListEnum = default;
        }


        public NativeList<TResult> ToNativeList(Allocator allocator)
        {
            var srcList = source.ToNativeList(Allocator.Temp);
            var groupMap = new NativeMultiHashMap<TKey, TElement>(srcList.Length, Allocator.Temp);
            var result = ToNativeList(srcList, groupMap, allocator);
            srcList.Dispose();
            groupMap.Dispose();
            return result;
        }

        public NativeList<TResult> ToNativeList(NativeList<T> srcList, NativeMultiHashMap<TKey, TElement> groupMap, Allocator allocator)
        {
            var results = new NativeList<TResult>(allocator);
            AddElementsToGroupMap(in srcList, ref groupMap);
            EnumerateGroupsAndAddToResults(in groupMap, ref results);
            return results;
        }

        private void AddElementsToGroupMap(in NativeList<T> srcList, ref NativeMultiHashMap<TKey, TElement> groupMap)
        {
            for (var i = 0; i < srcList.Length; i++)
            {
                var item = srcList[i];
                var key = keySelector.Invoke(item);
                var element = elementSelector.Invoke(item);
                groupMap.Add(key, element);
            }
        }

        private void EnumerateGroupsAndAddToResults(in NativeMultiHashMap<TKey, TElement> groupMap, ref NativeList<TResult> results)
        {
            var keysArr = groupMap.GetKeyArray(Allocator.Temp);
            var keySet = new NativeHashSet<TKey>(keysArr.Length, Allocator.Temp);
            for (var i = 0; i < keysArr.Length; i++)
            {
                var key = keysArr[i];
                if (keySet.Add(key))
                {
                    var result = resultSelector.Invoke(key, groupMap);
                    results.Add(result);
                }
            }
            keysArr.Dispose();
            keySet.Dispose();
        }

        public bool MoveNext()
        {
            if (!resultList.IsCreated)
            {
                resultList = ToNativeList(Allocator.Temp);
                resultListEnum = resultList.GetEnumerator();
            }
            return resultListEnum.MoveNext();
        }

        public void Reset()
        {
            throw new NotSupportedException();
        }

        public void Dispose()
        {
            source.Dispose();
            if (resultList.IsCreated)
                resultList.Dispose();
        }
    }

    public static class GroupBySequence
    {
        public static GroupBySequence<T, TSource, TKey, TKeySelector, TElement, TElementSelector, TResult, TResultSelector> New<T, TSource, TKey, TKeySelector, TElement, TElementSelector, TResult, TResultSelector>(
            ref TSource source,
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
            return new GroupBySequence<T, TSource, TKey, TKeySelector, TElement, TElementSelector, TResult, TResultSelector>(ref source, keySelector, elementSelector, resultSelector);
        }
    }
}
