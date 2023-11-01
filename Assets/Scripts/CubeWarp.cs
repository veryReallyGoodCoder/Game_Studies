using UnityEngine;

public class CubeWarp : MonoBehaviour
{

    Mesh mesh;
    Vector3[] originalVertices;
    Vector3[] modifiedVertices;

    public float meltSpeed = 1f;
    public float amplitude = 0.5f;
    public float frequency = 1f;
    
    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        originalVertices = mesh.vertices;
        modifiedVertices = new Vector3[originalVertices.Length];
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < originalVertices.Length; i++)
        {
            Vector3 vertex = originalVertices[i];
            vertex.y += Mathf.Sin(Time.time * frequency + vertex.x + vertex.z) * amplitude * Time.deltaTime * meltSpeed;
            modifiedVertices[i] = vertex;
        }

        mesh.vertices = modifiedVertices;
        mesh.RecalculateNormals();
    }
}
