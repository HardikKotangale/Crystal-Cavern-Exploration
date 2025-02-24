Shader "Custom/GlowingCrystalsShader"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (1, 0, 1, 1)
        _PulseSpeed ("Pulse Speed", Float) = 2.0
        _LightColor ("Light Color", Color) = (0, 0, 0, 0)
        _LightIntensity ("Light Intensity", Float) = 0.8
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
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
            };

            fixed4 _BaseColor;
            float _PulseSpeed;
            fixed4 _LightColor;
            float _LightIntensity;
            float3 _LightPosition;

            v2f vert (appdata_t v)
            {
                v2f o;

                // Transform to clip space
                o.vertex = UnityObjectToClipPos(v.vertex);

                // Pass world position and normal to the fragment shader
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.worldNormal = normalize(mul((float3x3)unity_ObjectToWorld, v.normal));

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Compute pulsating intensity
                float pulse = (sin(_Time * _PulseSpeed) + 1.0) * 0.5; // Pulsates between 0 and 1
                fixed4 dynamicBaseColor = _BaseColor * pulse;

                // Compute light direction and intensity
                float3 lightDir = normalize(_LightPosition - i.worldPos);
                float diff = saturate(dot(i.worldNormal, lightDir)); // Diffuse lighting

                // Apply light intensity and color with pulsating effect
                fixed4 dynamicLightColor = _LightColor * (_LightIntensity * diff * pulse);

                // Combine dynamic base color and dynamic light color
                fixed4 finalColor = dynamicBaseColor + dynamicLightColor;

                // Modulate the alpha of the final color based on pulse
                finalColor.a = pulse;

                return finalColor;
            }
            ENDCG
        }
    }
}