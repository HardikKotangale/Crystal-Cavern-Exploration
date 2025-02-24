Shader "Custom/TorusInteractionShader"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (1, 1, 1, 1) // Default torus color
        _InteractionColor ("Interaction Color", Color) = (0, 1, 0, 1) // Color when near the light
        _LightPosition ("Light Position", Vector) = (0, 0, 0) // Position of the glowing sphere
        _InteractionRadius ("Interaction Radius", Float) = 1.0 // Distance for color change
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
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD0;
            };

            float4 _BaseColor;
            float4 _InteractionColor;
            float3 _LightPosition;
            float _InteractionRadius;

            v2f vert (appdata_t v)
            {
                v2f o;

                // Transform vertex to clip space
                o.vertex = UnityObjectToClipPos(v.vertex);

                // Pass world position to the fragment shader
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Calculate the distance to the light source
                float distanceToLight = distance(i.worldPos, _LightPosition);

                // Calculate the lerp factor (0 when far, 1 when close)
                float lerpFactor = saturate(1.0 - (distanceToLight / _InteractionRadius));

                // Interpolate between base color and interaction color
                fixed4 color = lerp(_BaseColor, _InteractionColor, lerpFactor);

                return color;
            }
            ENDCG
        }
    }
}
