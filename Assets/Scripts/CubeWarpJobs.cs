using Unity.Collections;
using Unity.Burst;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.UIElements;

public class CubeWarpJobs : MonoBehaviour
{

    Mesh mesh;
    NativeArray<Vector3> originalVertices;
    NativeArray<Vector3> modifiedVertices;

    public float warpSpeed = 1f;
    public float amplitude = 0.5f;
    public float frequency = 1f;

    JobHandle warpJobHandle;

    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        originalVertices = new NativeArray<Vector3>(mesh.vertices, Allocator.Persistent);
        modifiedVertices = new NativeArray<Vector3>(originalVertices.Length, Allocator.Persistent);
    }

    // Update is called once per frame
    void Update()
    {
        WarpJob warpJob = new WarpJob
        {
            _originalVertices = originalVertices,
            _modifiedVertices = modifiedVertices,
            _time = Time.time,
            _warpSpeed = warpSpeed,
            _amplitude = amplitude,
            _frequency = frequency,
        };

        warpJobHandle = warpJob.Schedule(originalVertices.Length, 64);

        warpJobHandle.Complete();

        mesh.vertices = modifiedVertices.ToArray();
        mesh.RecalculateNormals();
    }

    private void LateUpdate()
    {
        for(int i = 0; i < originalVertices.Length; i++)
        {
            modifiedVertices[i] = originalVertices[i];
        }
    }

    private void OnDestroy()
    {
        originalVertices.Dispose();
        modifiedVertices.Dispose();
    }

    [BurstCompile]

    struct WarpJob : IJobParallelFor
    {
        [ReadOnly] public NativeArray<Vector3> _originalVertices;
        public NativeArray<Vector3> _modifiedVertices;

        public float _time;
        public float _warpSpeed;
        public float _amplitude;
        public float _frequency;

        public void Execute(int index)
        {
            Vector3 vertex = _originalVertices[index];

            vertex.y += Mathf.Sin(_time * _frequency + vertex.x + vertex.z) * _amplitude * _warpSpeed;
            _modifiedVertices[index] = vertex;
        }
    }
}
