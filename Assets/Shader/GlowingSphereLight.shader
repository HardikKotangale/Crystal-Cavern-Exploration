Shader "Custom/GlowingSphereLight"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (1, 1, 1, 1) // Base color for the sphere
        _AmbientColor ("Ambient Color", Color) = (1, 1, 1, 1) // Color of the ambient light
        _LightColor ("Light Color", Color) = (1, 1, 0, 1) // Color of the diffuse light
        _LightPosition ("Light Position", Vector) = (0, 1, 0, 1) // Position of the light source
        _GlowIntensity ("Glow Intensity", Float) = 1.0 // Intensity of the glowing effect
        _AmbientIntensity ("Ambient Intensity", Float) = 0.5 // Intensity of the ambient light
        _DiffuseIntensity ("Diffuse Intensity", Float) = 0.8 // Intensity of the diffuse light
        _SpecularIntensity ("Specular Intensity", Float) = 0.6 // Intensity of the specular reflection
        _Shininess ("Shininess", Float) = 32.0 // Controls the size of the specular highlight
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldNormal : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
            };

            float4 _BaseColor;
            float4 _AmbientColor;
            float4 _LightColor;
            float4 _LightPosition;
            float _GlowIntensity;
            float _AmbientIntensity;
            float _DiffuseIntensity;
            float _SpecularIntensity;
            float _Shininess;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldNormal = normalize(mul((float3x3)unity_ObjectToWorld, v.normal));
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Calculate light direction
                float3 lightDir = normalize(_LightPosition.xyz - i.worldPos);

                // Ambient component
                fixed4 ambient = _AmbientIntensity * _AmbientColor * _BaseColor;

                // Diffuse component
                float diff = max(dot(i.worldNormal, lightDir), 0.0);
                fixed4 diffuse = _DiffuseIntensity * _LightColor * diff * _BaseColor;

                // Specular component
                float3 viewDir = normalize(-i.worldPos); // View direction towards the camera
                float3 reflectDir = reflect(-lightDir, i.worldNormal); // Reflect light direction
                float spec = pow(max(dot(viewDir, reflectDir), 0.0), _Shininess);
                fixed4 specular = _SpecularIntensity * _LightColor * spec;

                // Combine lighting components
                fixed4 finalColor = ambient + diffuse + specular;

                // Apply glow effect
                finalColor.rgb += _GlowIntensity * _LightColor.rgb;

                return finalColor;
            }
            ENDCG
        }
    }
}
