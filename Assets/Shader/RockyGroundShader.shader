Shader "Custom/RockyGroundShaderWithLightFalloff"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "black" {}
        _BumpStrength ("Bump Strength", Float) = 0.5
        _NoiseScale ("Noise Scale", Float) = 5.0
        _TexScale ("Texture Scale", Float) = 1.0
        _LightColor ("Light Color", Color) = (0, 1, 0, 0) // Light Color
        _LightIntensity ("Light Intensity", Float) = 1.0 // Light Intensity
        _LightRange ("Light Range", Float) = 5.0 // Effective Range of the Light
        _SpecularStrength ("Specular Strength", Float) = 0.5 // Specular highlight strength
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
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD1;
                float3 worldNormal : TEXCOORD2;
            };

            sampler2D _MainTex;
            float _BumpStrength;
            float _NoiseScale;
            float _TexScale;
            fixed4 _LightColor;
            float _LightIntensity;
            float _LightRange;
            float _SpecularStrength;

            float3 _LightPosition; // Light source position in world space

            v2f vert (appdata_t v)
            {
                v2f o;

                // Add noise-based bump effect
                float noise = sin(v.vertex.x * _NoiseScale) * cos(v.vertex.z * _NoiseScale);
                v.vertex.y += noise * _BumpStrength;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.worldNormal = normalize(mul((float3x3)unity_ObjectToWorld, v.normal));
                o.uv = v.uv * _TexScale;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Sample base texture
                fixed4 baseColor = tex2D(_MainTex, i.uv);

                // Calculate distance from light source
                float distance = length(_LightPosition - i.worldPos);

                // Attenuation based on distance and light range
                float attenuation = saturate(1.0 - (distance / _LightRange));

                // Light direction
                float3 lightDir = normalize(_LightPosition - i.worldPos);

                // Diffuse lighting
                float diffuse = max(0, dot(i.worldNormal, lightDir)) * attenuation;

                // Specular highlight (Blinn-Phong)
                float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);
                float3 halfDir = normalize(lightDir + viewDir);
                float spec = pow(max(dot(i.worldNormal, halfDir), 0.0), 16.0) * _SpecularStrength * attenuation;

                // Combine diffuse and specular lighting with light color and intensity
                fixed4 lighting = _LightColor * (_LightIntensity * (diffuse + spec));

                // Combine lighting with the base texture
                return baseColor * lighting;
            }
            ENDCG
        }
    }
}
