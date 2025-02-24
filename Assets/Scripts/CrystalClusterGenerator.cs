using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalClusterGenerator : MonoBehaviour
{
    public GameObject plane; // Reference to the plane surface
    public int crystalCount = 10; // Number of crystals in the cluster
    public float maxHeight = 3f; // Maximum height of a crystal
    public Material crystalMaterial; // Material for the crystals
    public Material lightspherelMaterial;
    
    private List<Material> crystalMaterials = new List<Material>();

    void Start()
    {
        GenerateCluster();
    }

    void Update()
    {
        AnimateGlow();
    }

    void AnimateGlow()
    {
        float glowIntensity = Mathf.Abs(Mathf.Sin(Time.time)); // Sinusoidal animation
        foreach (Material mat in crystalMaterials)
        {
            if (mat.HasProperty("_GlowIntensity"))
            {
                mat.SetFloat("_GlowIntensity", glowIntensity);
            }
        }
    }

    void GenerateCluster()
    {
        if (plane == null)
        {
            Debug.LogError("No plane assigned for crystal placement!");
            return;
        }
        if (crystalMaterial == null)
        {
            Debug.LogError("No crystal material assigned!");
            return;
        }

        // Reference to the glowing sphere
        Transform glowingSphere = GameObject.Find("pointLight")?.transform;
        if (glowingSphere == null)
        {
            Debug.LogError("No object named 'GlowingSphere' found in the scene!");
            return;
        }

        // Get plane bounds
        Renderer planeRenderer = plane.GetComponent<Renderer>();
        Vector3 planeMin = planeRenderer.bounds.min;
        Vector3 planeMax = planeRenderer.bounds.max;

        for (int i = 0; i < crystalCount; i++)
        {
            // Generate a random position within the plane bounds
            float randomX = Random.Range(planeMin.x, planeMax.x);
            float randomZ = Random.Range(planeMin.z, planeMax.z);
            Vector3 position = new Vector3(randomX, planeMin.y, randomZ);

            // Random height for the prism
            float height = Random.Range(1f, maxHeight);

            // Create and configure the prism
            GameObject crystal = GeneratePrism(height);
            crystal.transform.position = position;
            crystal.transform.rotation = Quaternion.Euler(Random.Range(0, 360), 0, 0); // Random rotation
            crystal.transform.parent = this.transform;

            // Apply material with texture
            Material matInstance = new Material(crystalMaterial);
            MeshRenderer renderer = crystal.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                renderer.material = matInstance;
                crystalMaterials.Add(matInstance);
            }
            else
            {
                Debug.LogError("MeshRenderer not found on generated crystal!");
            }

            // Add CrystalInteraction script to enable proximity detection
            CrystalInteraction interaction = crystal.AddComponent<CrystalInteraction>();
            interaction.lightSource = glowingSphere;
            interaction.lightspherelMaterial=lightspherelMaterial;

             // Add the DragObject script to the crystal
            DragObject dragScript = crystal.AddComponent<DragObject>();

            // Enable the script
            dragScript.enabled = true;

            // Set the InteractionMode to 3 for rotation
            dragScript.SetInteractionMode(3);
        }
    }

    GameObject GeneratePrism(float height)
    {
        // Create a new GameObject with MeshFilter and MeshRenderer
        GameObject prism = new GameObject("Crystal");
        MeshFilter mf = prism.AddComponent<MeshFilter>();
        MeshRenderer mr = prism.AddComponent<MeshRenderer>();

        // Generate the prism mesh
        Mesh mesh = new Mesh();
        mf.mesh = mesh;

        // Define vertices for each face (each face needs unique vertices for independent UV mapping)
        float baseSize = 0.5f; // Size of the rectangular base
        Vector3[] vertices = new Vector3[]
        {
            // Bottom face
            new Vector3(-baseSize, 0, -baseSize), // 0
            new Vector3(baseSize, 0, -baseSize),  // 1
            new Vector3(baseSize, 0, baseSize),   // 2
            new Vector3(-baseSize, 0, baseSize),  // 3

            // Top face
            new Vector3(-baseSize, height, -baseSize), // 4
            new Vector3(baseSize, height, -baseSize),  // 5
            new Vector3(baseSize, height, baseSize),   // 6
            new Vector3(-baseSize, height, baseSize),  // 7

            // Side faces (duplicate vertices for independent UV mapping)
            new Vector3(-baseSize, 0, -baseSize), // 8
            new Vector3(-baseSize, height, -baseSize), // 9
            new Vector3(baseSize, 0, -baseSize), // 10
            new Vector3(baseSize, height, -baseSize), // 11

            new Vector3(baseSize, 0, -baseSize), // 12
            new Vector3(baseSize, height, -baseSize), // 13
            new Vector3(baseSize, 0, baseSize), // 14
            new Vector3(baseSize, height, baseSize), // 15

            new Vector3(baseSize, 0, baseSize), // 16
            new Vector3(baseSize, height, baseSize), // 17
            new Vector3(-baseSize, 0, baseSize), // 18
            new Vector3(-baseSize, height, baseSize), // 19

            new Vector3(-baseSize, 0, baseSize), // 20
            new Vector3(-baseSize, height, baseSize), // 21
            new Vector3(-baseSize, 0, -baseSize), // 22
            new Vector3(-baseSize, height, -baseSize) // 23
        };

        // Define triangles for all faces of the prism
        int[] triangles = new int[]
        {
            // Bottom face
            0, 1, 2,
            2, 3, 0,

            // Top face
            4, 7, 6,
            6, 5, 4,

            // Side faces
            8, 9, 11, 11, 10, 8, // Side 1
            12, 13, 15, 15, 14, 12, // Side 2
            16, 17, 19, 19, 18, 16, // Side 3
            20, 21, 23, 23, 22, 20  // Side 4
        };

        // Define UV coordinates for each face's vertices
        Vector2[] uv = new Vector2[]
        {
            // Bottom face
            new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1),

            // Top face
            new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1),

            // Side faces
            new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 0), new Vector2(1, 1), // Side 1
            new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 0), new Vector2(1, 1), // Side 2
            new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 0), new Vector2(1, 1), // Side 3
            new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 0), new Vector2(1, 1)  // Side 4
        };

        // Assign data to the mesh
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.RecalculateNormals();

        // Add a Box Collider for collision detection
        BoxCollider boxCollider = prism.AddComponent<BoxCollider>();
        boxCollider.center = new Vector3(0, height / 2, 0); // Center the collider vertically
        boxCollider.size = new Vector3(baseSize * 2, height, baseSize * 2); // Match the prism dimensions

        return prism;
}


}
