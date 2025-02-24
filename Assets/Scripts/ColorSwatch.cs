using UnityEngine;
using UnityEngine.UI;

public class ColorSwatch : MonoBehaviour
{
    public Material glowingSphereMaterial; // Reference to the glowing sphere material
    public Material rockyMaterial;        // Reference to the rocky ground material
    public Material torusMaterial;        // Reference to the torus  material

    // UI Buttons for Color Swatches
    public Button redButton;
    public Button greenButton;
    public Button blueButton;
    public Button yellowButton;

    private Color selectedColor;

    void Start()
    {
        // Add listener for each button to update the color
        redButton.onClick.AddListener(() => ChangeLightColor(Color.red));
        greenButton.onClick.AddListener(() => ChangeLightColor(Color.green));
        blueButton.onClick.AddListener(() => ChangeLightColor(Color.blue));
        yellowButton.onClick.AddListener(() => ChangeLightColor(Color.yellow));

        // Set a default color
        ChangeLightColor(Color.white);
    }

    void ChangeLightColor(Color newColor)
    {
        selectedColor = newColor;

        // Update the glowing sphere's material color
        if (glowingSphereMaterial != null)
        {
            glowingSphereMaterial.SetColor("_LightColor", selectedColor);
        }

        // Update the rocky material dynamically
        if (rockyMaterial != null)
        {
            rockyMaterial.SetColor("_LightColor", selectedColor);
        }
        // Update the torus material dynamically
        if (rockyMaterial != null)
        {
 
            torusMaterial.SetColor("_InteractionColor", selectedColor);
           
               
           
        }

        Debug.Log("Light Color Changed to: " + selectedColor);
    }
}
