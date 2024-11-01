Shader "Unlit/GridShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _FillColor ("FillColor", Color) = (0.0, 0.0, 0.0, 0.0)
        _LineColor ("LineColor", Color) = (1.0, 0.0, 0.0, 0.0)
        _GridSize ("GridSize", Float) = 0.5
        _LineThickness ("LineThickness", Float) = 0.005
        
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100 //level of detail

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog
            //#pragma enable_d3d11_debug_symbols

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _FillColor;
            float4 _LineColor;
            float _LineThickness;
            float _GridSize;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            float4 GridTest(float2 pixPos){
                
                //pixPos is stored in the form of x, y on the uv grid

                float3 worldScale = float3(
                    length(float3(unity_ObjectToWorld[0].x, unity_ObjectToWorld[1].x, unity_ObjectToWorld[2].x)), // scale x axis
                    length(float3(unity_ObjectToWorld[0].y, unity_ObjectToWorld[1].y, unity_ObjectToWorld[2].y)), // scale y axis
                    length(float3(unity_ObjectToWorld[0].z, unity_ObjectToWorld[1].z, unity_ObjectToWorld[2].z))  // scale z axis
                );

                bool onXLines = false;
                bool onYLines = false;

                // Loop through grid lines
                for (float lineLoc = 0.0; lineLoc < 1.0; lineLoc += _GridSize / worldScale.x ) {

                    float distX = abs(pixPos[0] - lineLoc); //the distance between the x uv coordinate of our pixel and the x line

                    if (distX <= _LineThickness){
                        onXLines = true;
                    }
                }    
                
                for (float lineLoc = 0.0; lineLoc < 1.0; lineLoc += _GridSize / worldScale.y) {

                    float distY = abs(pixPos[1] - lineLoc); //the same as above but y uv coordinate and the y line

                    if (distY <= _LineThickness){
                        onYLines = true;
                    }

                }
            
                if (onXLines || onYLines){
                    return _LineColor;
                }
                else{
                    return _FillColor;
                }
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                //fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                //UNITY_APPLY_FOG(i.fogCoord, col);
                return GridTest(i.uv);
            }
            ENDCG
        }
    }
}
