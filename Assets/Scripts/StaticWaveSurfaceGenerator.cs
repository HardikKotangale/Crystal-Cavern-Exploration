using UnityEngine;

public class StaticWaveSurfaceGenerator : MonoBehaviour
{
    public int width = 50;               // Number of horizontal subdivisions
    public int height = 50;              // Number of vertical subdivisions
    public float waveHeight = 1f;        // Amplitude of the waves
    public float waveFrequency = 2f;     // Frequency of the wave pattern
    public Material waveMaterial;        // Material to be applied to the mesh

    void Start()
    {
        GenerateStaticWaveMesh(); // Generate the plane with wave vertices
    }

    void GenerateStaticWaveMesh()
    {
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();

        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[(width + 1) * (height + 1)];
        Vector2[] uvs = new Vector2[(width + 1) * (height + 1)];
        int[] triangles = new int[width * height * 6];

        // Generate vertices with static wave heights
        for (int z = 0; z <= height; z++)
        {
            for (int x = 0; x <= width; x++)
            {
                float y = Mathf.Sin(x * waveFrequency) * waveHeight; // Static wave pattern
                vertices[z * (width + 1) + x] = new Vector3(x, y, z);

                // Assign UV coordinates
                uvs[z * (width + 1) + x] = new Vector2((float)x / width, (float)z / height);
            }
        }

        // Generate triangles
        int index = 0;
        for (int z = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                int start = z * (width + 1) + x;

                triangles[index++] = start;
                triangles[index++] = start + width + 1;
                triangles[index++] = start + 1;

                triangles[index++] = start + 1;
                triangles[index++] = start + width + 1;
                triangles[index++] = start + width + 2;
            }
        }

        // Assign mesh data
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        // Attach the mesh to the MeshFilter
        meshFilter.mesh = mesh;

        // Attach the material to the MeshRenderer
        if (waveMaterial != null)
        {
            meshRenderer.material = waveMaterial;
        }
        else
        {
            meshRenderer.material = new Material(Shader.Find("Standard"));
            Debug.LogWarning("No waveMaterial assigned. Using default Standard material.");
        }
    }
}
