using UnityEngine;

namespace Lab13_hkotanga
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class PlaneWaveGenerator : MonoBehaviour
    {
        public int width = 50;               // Number of horizontal subdivisions
        public int height = 50;              // Number of vertical subdivisions
        public float waveHeight = 0.5f;      // Amplitude of the wave
        public float waveFrequency = 1f;     // Frequency of the wave
        public float waveSpeed = 1f;         // Speed of the wave animation
        public Material waveMaterial;        // Reference to the material with a texture

        private MeshFilter meshFilter;
        private Mesh planeMesh;
        private Vector3[] originalVertices;
        private Vector3[] displacedVertices;

        void Start()
        {
            meshFilter = GetComponent<MeshFilter>();
            GeneratePlane(); // Create a custom plane

            // Cache the original vertices
            originalVertices = planeMesh.vertices;
            displacedVertices = new Vector3[originalVertices.Length];

            // Attach the material to the MeshRenderer
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            if (waveMaterial != null)
            {
                meshRenderer.material = waveMaterial;
            }
            else
            {
                Debug.LogWarning("No material assigned to 'waveMaterial'. Assign one in the inspector.");
            }
        }

        void Update()
        {
            AnimateWave();
        }

        void GeneratePlane()
        {
            planeMesh = new Mesh();

            Vector3[] vertices = new Vector3[(width + 1) * (height + 1)];
            Vector2[] uvs = new Vector2[(width + 1) * (height + 1)];
            int[] triangles = new int[width * height * 6];

            // Generate vertices and UVs
            for (int z = 0; z <= height; z++)
            {
                for (int x = 0; x <= width; x++)
                {
                    float y = Mathf.Sin(x * waveFrequency) * waveHeight; // Sine wave for Y
                    int index = z * (width + 1) + x;

                    vertices[index] = new Vector3(x, y, z);
                    uvs[index] = new Vector2((float)x / width, (float)z / height); // Normalize UV coordinates
                }
            }

            // Generate triangles
            int triIndex = 0;
            for (int z = 0; z < height; z++)
            {
                for (int x = 0; x < width; x++)
                {
                    int start = z * (width + 1) + x;

                    triangles[triIndex++] = start;
                    triangles[triIndex++] = start + width + 1;
                    triangles[triIndex++] = start + 1;

                    triangles[triIndex++] = start + 1;
                    triangles[triIndex++] = start + width + 1;
                    triangles[triIndex++] = start + width + 2;
                }
            }

            // Assign mesh data
            planeMesh.vertices = vertices;
            planeMesh.uv = uvs; // Assign UV coordinates
            planeMesh.triangles = triangles;
            planeMesh.RecalculateNormals();
            planeMesh.RecalculateBounds();
            meshFilter.mesh = planeMesh;
        }

        void AnimateWave()
        {
            // Animate the vertices of the plane to create a wave effect
            for (int i = 0; i < originalVertices.Length; i++)
            {
                Vector3 original = originalVertices[i];
                float wave = Mathf.Sin((original.x + original.z + Time.time * waveSpeed) * waveFrequency) * waveHeight;
                displacedVertices[i] = new Vector3(original.x, wave, original.z);
            }

            // Update the mesh with displaced vertices
            planeMesh.vertices = displacedVertices;
            planeMesh.RecalculateNormals();
            meshFilter.mesh = planeMesh;
        }
    }
}
