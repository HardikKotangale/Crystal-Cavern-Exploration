using UnityEngine;

public class GlowingSphereLightManager : MonoBehaviour
{
    public Light glowingSphereLight; // The glowing sphere's light
    public Material rockyMaterial;  // The material using the RockyGroundReflectiveShader
    public Material  crystalMaterial;  // The material using the CrystalShader
    public Material  torusMaterial;  // The material using the TorusShader
    void Update()
    {
        if (glowingSphereLight != null && rockyMaterial != null && crystalMaterial != null)
        {
            // Pass the glowing sphere's position to the shader
            rockyMaterial.SetVector("_LightPosition", glowingSphereLight.transform.position);

           
             // Pass the glowing sphere's position to the shader
            crystalMaterial.SetVector("_LightPosition", glowingSphereLight.transform.position);

            // Pass the glowing sphere's position to the shader
            torusMaterial.SetVector("_LightPosition", glowingSphereLight.transform.position);



        }
    }
}
