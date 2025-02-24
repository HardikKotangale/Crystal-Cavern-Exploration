using UnityEngine;

public class CrystalInteraction : MonoBehaviour
{
    public Transform lightSource; // Reference to the glowing sphere
    public float interactionRadius = 4f; // Distance within which interaction occurs
    private Material crystalMaterial; // Material of the crystal
    public Material lightspherelMaterial; // Material of the light sphere
    public float pulseSpeed = 4.0f; // Speed of pulsating effect

    private Color originalBaseColor; // Store the original base color

    void Start()
    {
        if (lightSource == null)
        {
            Debug.LogError($"lightSource is not assigned for {gameObject.name}!");
            return;
        }

        MeshRenderer renderer = GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            crystalMaterial = renderer.material;

            // Validate shader properties
            if (!crystalMaterial.HasProperty("_BaseColor") || !crystalMaterial.HasProperty("_LightColor") || !crystalMaterial.HasProperty("_LightIntensity"))
            {
                Debug.LogError("The assigned material does not have the required shader properties (_BaseColor, _LightColor, _LightIntensity).");
            }

            // Store the original base color
            originalBaseColor = crystalMaterial.GetColor("_BaseColor");
        }
    }

    void Update()
    {
        // Check if the glowing sphere (lightSource) is active
        if (lightSource != null && lightSource.gameObject.activeSelf)
        {
            StartPulsating(); // Pulsate when the light source is active
        }
        else
        {
            StopPulsating(); // Stop pulsation and reset color when light is off
        }
    }

    public void StartPulsating()
    {
        if (lightSource == null || crystalMaterial == null) return;

        // Calculate the distance to the light source
        float distance = Vector3.Distance(transform.position, lightSource.position);

        // Compute pulsating intensity
        float pulse = (Mathf.Sin(Time.time * pulseSpeed) + 1.0f) * 0.5f; // Pulsates between 0 and 1

        // Update shader properties if within interaction radius
        if (distance <= interactionRadius)
        {
            float lerpFactor = 1 - (2*(distance / interactionRadius)); // Closer = brighter

            // Compute dynamic color blending with red tint
            Color lightColor = crystalMaterial.GetColor("_LightColor");
            Color redColor = lightspherelMaterial.GetColor("_LightColor"); // Red color when light source is close
            Color blendedColor = Color.Lerp(redColor,redColor, lerpFactor) * pulse;

            // Update material properties
            crystalMaterial.SetColor("_BaseColor", blendedColor);
            crystalMaterial.SetFloat("_LightIntensity", pulse * lerpFactor); // Adjust light intensity with proximity and pulse
        }
        else
        {
            // Reset to the original base color and minimal intensity when out of range
            crystalMaterial.SetColor("_BaseColor", originalBaseColor);
            crystalMaterial.SetFloat("_LightIntensity", 0.0f);
        }
    }
    
    public void StopPulsating()
    {
        if (crystalMaterial != null)
        {
            // Reset the light intensity and base color
            crystalMaterial.SetFloat("_LightIntensity", 0.0f);
            crystalMaterial.SetColor("_BaseColor", originalBaseColor);
        }
    }
}
