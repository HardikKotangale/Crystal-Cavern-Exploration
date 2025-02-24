using UnityEngine;

public class TorusKnotGenerator : MonoBehaviour
{
    public int radialSegments = 36;
    public int tubularSegments = 18;
    public float majorRadius = 2f;
    public float minorRadius = 0.5f;
    public Material torusMaterial;
    public Material lightMaterial;
    public Transform glowingLight; // Reference to the glowing light source
    public GameObject glowingSphere;
    public Vector3 torusPosition = new Vector3(-7.32f, 0.37f, -18f);
    
    public float interactionRadius =2f; // Radius for interaction with glowing light
    private GameObject generatedTorus; // Store reference to the generated torus
    private MaterialPropertyBlock propertyBlock;

void Start()
{
    propertyBlock = new MaterialPropertyBlock();
    GenerateTorus(torusPosition);
}

void Update()
{
    if (glowingLight != null)
    {
        // Update the light position dynamically for the generated torus
        UpdateLightPosition(glowingLight.position);
    }
}

public void UpdateLightPosition(Vector3 lightPosition)
{
    if (generatedTorus != null)
    {
        Renderer renderer = generatedTorus.GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            renderer.GetPropertyBlock(propertyBlock);
            propertyBlock.SetVector("_LightPosition", lightPosition);
            renderer.SetPropertyBlock(propertyBlock);
        }
    }
}

public void GenerateTorus(Vector3 position)
{
    // Create the Torus GameObject
    generatedTorus = new GameObject("Torus");
    MeshFilter mf = generatedTorus.AddComponent<MeshFilter>();
    MeshRenderer mr = generatedTorus.AddComponent<MeshRenderer>();

    generatedTorus.transform.position = position;
    generatedTorus.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 90));

    Mesh mesh = new Mesh();
    mf.mesh = mesh;

    Vector3[] vertices = new Vector3[(radialSegments + 1) * (tubularSegments + 1)];
    int[] triangles = new int[radialSegments * tubularSegments * 6];

    int vertIndex = 0;
    for (int i = 0; i <= radialSegments; i++)
    {
        float radialAngle = 2 * Mathf.PI * i / radialSegments;
        Vector3 ringCenter = new Vector3(
            Mathf.Cos(radialAngle) * majorRadius,
            0,
            Mathf.Sin(radialAngle) * majorRadius
        );

        for (int j = 0; j <= tubularSegments; j++)
        {
            float tubularAngle = 2 * Mathf.PI * j / tubularSegments;
            Vector3 vertex = new Vector3(
                Mathf.Cos(tubularAngle) * minorRadius,
                Mathf.Sin(tubularAngle) * minorRadius,
                0
            );

            vertex = Quaternion.Euler(0, -radialAngle * Mathf.Rad2Deg, 0) * vertex + ringCenter;
            vertices[vertIndex++] = vertex;
        }
    }

    int triIndex = 0;
    for (int i = 0; i < radialSegments; i++)
    {
        for (int j = 0; j < tubularSegments; j++)
        {
            int current = i * (tubularSegments + 1) + j;
            int next = current + tubularSegments + 1;

            triangles[triIndex++] = current;
            triangles[triIndex++] = next;
            triangles[triIndex++] = current + 1;

            triangles[triIndex++] = current + 1;
            triangles[triIndex++] = next;
            triangles[triIndex++] = next + 1;
        }
    }

    mesh.vertices = vertices;
    mesh.triangles = triangles;
    mesh.RecalculateBounds();
    mesh.RecalculateNormals();

    mr.material = torusMaterial;

    // Store reference to this torus
    generatedTorus = mr.gameObject;

    // Add the TorusInteraction component dynamically
    TorusInteraction torusInteraction = generatedTorus.AddComponent<TorusInteraction>();
    torusInteraction.lightSource = glowingLight;
    torusInteraction.interactionRadius = interactionRadius;
    torusInteraction.glowingSphere = glowingSphere;
    torusInteraction.lightSphereMaterial = lightMaterial;
}
}
