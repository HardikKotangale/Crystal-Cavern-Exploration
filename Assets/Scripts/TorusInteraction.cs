using UnityEngine;

public class TorusInteraction : MonoBehaviour
{
   
    public Transform lightSource; // Reference to the glowing sphere
    public GameObject glowingSphere; // Reference to the glowing sphere GameObject
    public Material lightSphereMaterial; // Material of the glowing sphere (to get the light color)


    public float interactionRadius = 2f; // Radius for interaction with the light source
    public Color interactionColor; // Color to blend when close
    public Color offColor = Color.black; // Color when the light is off

    private Material torusMaterial; // Torus material
    private Color originalColor; // Store the original color

    void Start()
    {
        // Validate the light source
        if (lightSource == null || lightSphereMaterial == null)
        {
            Debug.LogError($"lightSource or lightSphereMaterial is not assigned for {gameObject.name}!");
            return;
        }

        // Get the material of the torus
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            torusMaterial = renderer.material;

            // Validate if the material has "_BaseColor" property
            if (torusMaterial.HasProperty("_BaseColor"))
            {
                originalColor = torusMaterial.GetColor("_BaseColor");
            }
            else
            {
                Debug.LogError("Material is missing the '_BaseColor' property.");
            }
        }
    }

    void Update()
    {
        // If the light is off or inactive, set torus color to black
        if (glowingSphere == null || !glowingSphere.activeSelf)
        {
            SetTorusColor(offColor);
            torusMaterial.SetColor("_InteractionColor", offColor); // Update interaction color
            return;
        }

        // Calculate distance from the torus to the light source
        float distance = Vector3.Distance(transform.position, lightSource.position);

        if (distance <= interactionRadius)
        {
            // Calculate lerp factor based on proximity (1 = close, 0 = far)
            float lerpFactor = 1 - (distance / interactionRadius);

            // Get the current light color from the glowing sphere's material
            Color lightColor = lightSphereMaterial.GetColor("_LightColor");

            // Blend the torus color between its original color and interaction color
            Color blendedColor = Color.Lerp(originalColor, interactionColor, lerpFactor);
            SetTorusColor(blendedColor);

            // Dynamically set the interaction color based on the light's color
            if (torusMaterial.HasProperty("_InteractionColor"))
            {
                torusMaterial.SetColor("_InteractionColor", lightColor);
            }
        }
        else
        {
            // Reset to original color when out of range
            SetTorusColor(originalColor);
        }
    }

    // Helper method to set the torus material's color
    private void SetTorusColor(Color newColor)
    {
        if (torusMaterial != null && torusMaterial.HasProperty("_BaseColor"))
        {
            torusMaterial.SetColor("_BaseColor", newColor);
        }
    }
}
