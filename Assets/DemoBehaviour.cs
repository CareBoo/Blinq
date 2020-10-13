using Unity.Jobs;
using UnityEngine;
using CareBoo.Blinq;
using Unity.Collections;

public class DemoBehaviour : MonoBehaviour
{
    public struct SelectJob<TSelector> : IJob
        where TSelector : struct, IFunc<int, long>
    {
        [ReadOnly]
        public NativeArray<int> Source;

        public ValueFunc<int, long>.Impl<TSelector> Selector;

        [WriteOnly]
        public NativeArray<long> Output;

        public void Execute()
        {
            Source.Select(Selector).ToNativeArray(Output);
        }
    }

    public struct AddOne : IFunc<int, long>
    {
        public long Invoke(int val) => val + 1;
    }

    NativeArray<int> source;

    void Awake()
    {
        source = new NativeArray<int>(new int[] { 1, 2, 3 }, Allocator.Persistent);
    }

    void Start()
    {
        var output = new NativeArray<long>(source.Length, Allocator.Persistent);
        var job = new SelectJob<AddOne> { Source = source, Selector = ValueFunc<int, long>.CreateImpl<AddOne>(), Output = output };
        job.Run();
        Debug.Log($"output: ({string.Join(",", output)})");
        output.Dispose();
    }

    void OnDestroy()
    {
        source.Dispose();
    }
}
