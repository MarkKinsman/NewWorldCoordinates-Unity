// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Hyperreal/StandardBoxMaskTransparent"
{
	Properties
	{
		[NoScaleOffset]_MainTex("Albedo", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
		[NoScaleOffset]_MetallicGlossMap("Metallic", 2D) = "white" {}
		_Metallic("Metallic", Range( 0 , 1)) = 0
		_Glossiness("Smoothness", Range( 0 , 1)) = 0
		[NoScaleOffset]_OcclusionMap("Occlusion", 2D) = "white" {}
		[NoScaleOffset]_EmissionMap("EmissionMap", 2D) = "white" {}
		[Toggle]_Emission("Emission", Float) = 0
		[HDR]_EmissionColor("EmissionColor", Color) = (0,0,0,0)
		_OcclusionStrength("Strength", Range( 0 , 1)) = 0
		_Tiling("Tiling", Vector) = (1,1,0,0)
		_Offset("Offset", Vector) = (0,0,0,0)
		[Toggle]_OffsetInLocalSpace("Offset In Local Space", Float) = 0
		_OffsetPosition("OffsetPosition", Vector) = (0,0,0,0)
		_OffsetScale("OffsetScale", Vector) = (100,100,100,0)
		_NormalizedRotationAxis("NormalizedRotationAxis", Vector) = (0,0,0,0)
		_RotationAngle("RotationAngle", Float) = 0
		_PivotPoint("PivotPoint", Vector) = (0,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
		};

		uniform sampler2D _MainTex;
		uniform float2 _Tiling;
		uniform float2 _Offset;
		uniform float4 _Color;
		uniform float _Emission;
		uniform sampler2D _EmissionMap;
		uniform float4 _EmissionColor;
		uniform float _Metallic;
		uniform sampler2D _MetallicGlossMap;
		uniform float _Glossiness;
		uniform sampler2D _OcclusionMap;
		uniform float _OcclusionStrength;
		uniform float3 _NormalizedRotationAxis;
		uniform float _RotationAngle;
		uniform float3 _PivotPoint;
		uniform float _OffsetInLocalSpace;
		uniform float3 _OffsetPosition;
		uniform float3 _OffsetScale;


		float3 RotateAroundAxis( float3 center, float3 original, float3 u, float angle )
		{
			original -= center;
			float C = cos( angle );
			float S = sin( angle );
			float t = 1 - C;
			float m00 = t * u.x * u.x + C;
			float m01 = t * u.x * u.y - S * u.z;
			float m02 = t * u.x * u.z + S * u.y;
			float m10 = t * u.x * u.y + S * u.z;
			float m11 = t * u.y * u.y + C;
			float m12 = t * u.y * u.z - S * u.x;
			float m20 = t * u.x * u.z - S * u.y;
			float m21 = t * u.y * u.z + S * u.x;
			float m22 = t * u.z * u.z + C;
			float3x3 finalMatrix = float3x3( m00, m01, m02, m10, m11, m12, m20, m21, m22 );
			return mul( finalMatrix, original ) + center;
		}


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TexCoord158 = i.uv_texcoord * _Tiling + _Offset;
			float4 tex2DNode135 = tex2D( _MainTex, uv_TexCoord158 );
			o.Albedo = ( tex2DNode135 * _Color ).rgb;
			o.Emission = lerp(float4( 0,0,0,0 ),( tex2D( _EmissionMap, uv_TexCoord158 ) * _EmissionColor ),_Emission).rgb;
			float4 tex2DNode138 = tex2D( _MetallicGlossMap, uv_TexCoord158 );
			o.Metallic = ( _Metallic * tex2DNode138 ).r;
			o.Smoothness = ( _Glossiness * tex2DNode138.a );
			o.Occlusion = ( tex2D( _OcclusionMap, uv_TexCoord158 ) * _OcclusionStrength ).r;
			float3 ase_worldPos = i.worldPos;
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float3 rotatedValue129 = RotateAroundAxis( _PivotPoint, (lerp(ase_worldPos,ase_vertex3Pos,_OffsetInLocalSpace)*1.0 + ( _OffsetPosition * -1.0 )), _NormalizedRotationAxis, _RotationAngle );
			float3 temp_output_77_0 = (rotatedValue129*( 1.0 / _OffsetScale ) + 0.0);
			float dotResult86 = dot( temp_output_77_0 , float3(0,-1,0) );
			float blendOpSrc87 = dotResult86;
			float blendOpDest87 = ( dotResult86 * -1.0 );
			float dotResult90 = dot( temp_output_77_0 , float3(-1,0,0) );
			float blendOpSrc92 = dotResult90;
			float blendOpDest92 = ( dotResult90 * -1.0 );
			float dotResult95 = dot( temp_output_77_0 , float3(0,0,-1) );
			float blendOpSrc97 = dotResult95;
			float blendOpDest97 = ( dotResult95 * -1.0 );
			float blendOpSrc99 = ( saturate( 	max( blendOpSrc92, blendOpDest92 ) ));
			float blendOpDest99 = ( saturate( 	max( blendOpSrc97, blendOpDest97 ) ));
			float blendOpSrc98 = ( saturate( 	max( blendOpSrc87, blendOpDest87 ) ));
			float blendOpDest98 = ( saturate( 	max( blendOpSrc99, blendOpDest99 ) ));
			float blendOpSrc192 = ( 1.0 - ( saturate( 	max( blendOpSrc98, blendOpDest98 ) )) );
			float blendOpDest192 = 0.5;
			o.Alpha = ( ( tex2DNode135.a * _Color.a ) * ( saturate(  round( 0.5 * ( blendOpSrc192 + blendOpDest192 ) ) )) );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15800
1927;452;1586;788;233.0277;1998.772;1.826521;True;True
Node;AmplifyShaderEditor.CommentaryNode;124;-671.4299,-613.6932;Float;False;663.5178;362.847;Offset Position;4;70;62;60;63;;1,1,1,1;0;0
Node;AmplifyShaderEditor.WorldPosInputsNode;164;-1093.914,-568.1384;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.PosVertexDataNode;165;-1101.071,-429.2835;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector3Node;70;-605.2819,-481.4611;Float;False;Property;_OffsetPosition;OffsetPosition;13;0;Create;True;0;0;True;0;0,0,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;63;-599.7238,-341.1241;Float;False;Constant;_PositionMultiplier;PositionMultiplier;2;0;Create;True;0;0;False;0;-1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;62;-401.0847,-495.8198;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ToggleSwitchNode;166;-872.0337,-571.0007;Float;False;Property;_OffsetInLocalSpace;Offset In Local Space;12;0;Create;False;0;0;False;0;0;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;134;-671.4598,-202.2677;Float;False;664.4344;506.9017;Offset Rotation;4;129;130;131;132;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;127;-86.56047,351.7419;Float;False;773.9248;282.3134;Offset Scale;4;77;74;75;72;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;74;148.7421,419.0046;Float;False;Constant;_ScaleOne;ScaleOne;4;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;130;-621.4597,-152.2677;Float;False;Property;_NormalizedRotationAxis;NormalizedRotationAxis;15;0;Create;True;0;0;False;0;0,0,0;0,1,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;131;-613.3856,22.13123;Float;False;Property;_RotationAngle;RotationAngle;16;0;Create;True;0;0;False;0;0;0.785398;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;133;105.9896,-620.3318;Float;False;1337.964;859.342;Blending;15;126;111;90;95;112;86;113;110;97;92;87;99;98;84;83;;1,1,1,1;0;0
Node;AmplifyShaderEditor.Vector3Node;132;-608.5412,120.6341;Float;False;Property;_PivotPoint;PivotPoint;17;0;Create;True;0;0;False;0;0,0,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ScaleAndOffsetNode;60;-230.9121,-563.6933;Float;False;3;0;FLOAT3;0,0,0;False;1;FLOAT;1;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.Vector3Node;72;-36.56044,452.0555;Float;False;Property;_OffsetScale;OffsetScale;14;0;Create;True;0;0;True;0;100,100,100;1.82594,5.0515,0.5;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RotateAboutAxisNode;129;-334.0252,1.138612;Float;False;False;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;126;155.9896,-405.876;Float;False;252;534.6279;Vector Directions;3;88;85;93;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;75;293.4763,425.1899;Float;False;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;77;464.3646,401.7419;Float;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;1,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.Vector3Node;88;208.7396,-355.876;Float;False;Constant;_VectorLeft;VectorLeft;4;0;Create;True;0;0;False;0;-1,0,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.Vector3Node;93;210.7398,-53.24832;Float;False;Constant;_VectorBack;VectorBack;4;0;Create;True;0;0;False;0;0,0,-1;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;111;446.5612,-116.9032;Float;False;Constant;_InverseMultiplier;InverseMultiplier;4;0;Create;True;0;0;False;0;-1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;95;697.1785,-26.76257;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;85;205.9896,-203.9345;Float;False;Constant;_VectorBottom;VectorBottom;4;0;Create;True;0;0;False;0;0,-1,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DotProductOpNode;90;690.6513,-476.824;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;86;694.0431,-247.0474;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;112;724.5921,-367.191;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;113;720.9764,101.8727;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;110;716.9278,-133.2352;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BlendOpsNode;92;868.3664,-482.1548;Float;True;Lighten;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BlendOpsNode;97;872.6304,-18.98968;Float;True;Lighten;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BlendOpsNode;87;873.2108,-245.7708;Float;True;Lighten;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BlendOpsNode;99;1164.953,-92.43018;Float;True;Lighten;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;159;124.0655,-1884.542;Float;False;Property;_Tiling;Tiling;10;0;Create;True;0;0;False;0;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;160;121.1377,-1757.181;Float;False;Property;_Offset;Offset;11;0;Create;True;0;0;False;0;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;158;299.7354,-1840.625;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BlendOpsNode;98;1164.123,-323.0221;Float;True;Lighten;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;84;1016.275,-570.3318;Float;False;Constant;_BlendWhite;BlendWhite;4;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;135;579.4346,-2055.456;Float;True;Property;_MainTex;Albedo;0;1;[NoScaleOffset];Create;False;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;142;691.5418,-1413.664;Float;False;Property;_EmissionColor;EmissionColor;8;1;[HDR];Create;True;0;0;False;0;0,0,0,0;0,0,0,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;137;620.491,-1600.98;Float;True;Property;_EmissionMap;EmissionMap;6;1;[NoScaleOffset];Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;83;1189.47,-549.8259;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;193;1536.871,-621.5754;Float;False;Constant;_Float3;Float 3;18;0;Create;True;0;0;False;0;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;149;666.3134,-1870.493;Float;False;Property;_Color;Color;1;0;Create;True;0;0;False;0;1,1,1,1;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;151;981.4779,-1091.741;Float;False;Property;_Glossiness;Smoothness;4;0;Create;False;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;147;983.785,-1216.903;Float;False;Property;_Metallic;Metallic;3;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;140;656.599,-885.3716;Float;True;Property;_OcclusionMap;Occlusion;5;1;[NoScaleOffset];Create;False;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;174;941.4187,-1730.268;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;138;676.8909,-1155.541;Float;True;Property;_MetallicGlossMap;Metallic;2;1;[NoScaleOffset];Create;False;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BlendOpsNode;192;1476.595,-813.3578;Float;False;HardMix;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;153;980.4779,-804.741;Float;False;Property;_OcclusionStrength;Strength;9;0;Create;False;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;163;938.7953,-1434.188;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;178;1818.152,-1132.995;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;152;1262.478,-1086.741;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;154;1249.478,-911.741;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;162;934.5484,-1974.163;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ToggleSwitchNode;148;1163.478,-1433.741;Float;False;Property;_Emission;Emission;7;0;Create;True;0;0;False;0;0;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;146;1259.834,-1218.518;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2011.516,-1442.959;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Hyperreal/StandardBoxMaskTransparent;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;Transparent2;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;1;False;-1;10;False;-1;21;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;12;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;62;0;70;0
WireConnection;62;1;63;0
WireConnection;166;0;164;0
WireConnection;166;1;165;0
WireConnection;60;0;166;0
WireConnection;60;2;62;0
WireConnection;129;0;130;0
WireConnection;129;1;131;0
WireConnection;129;2;132;0
WireConnection;129;3;60;0
WireConnection;75;0;74;0
WireConnection;75;1;72;0
WireConnection;77;0;129;0
WireConnection;77;1;75;0
WireConnection;95;0;77;0
WireConnection;95;1;93;0
WireConnection;90;0;77;0
WireConnection;90;1;88;0
WireConnection;86;0;77;0
WireConnection;86;1;85;0
WireConnection;112;0;90;0
WireConnection;112;1;111;0
WireConnection;113;0;95;0
WireConnection;113;1;111;0
WireConnection;110;0;86;0
WireConnection;110;1;111;0
WireConnection;92;0;90;0
WireConnection;92;1;112;0
WireConnection;97;0;95;0
WireConnection;97;1;113;0
WireConnection;87;0;86;0
WireConnection;87;1;110;0
WireConnection;99;0;92;0
WireConnection;99;1;97;0
WireConnection;158;0;159;0
WireConnection;158;1;160;0
WireConnection;98;0;87;0
WireConnection;98;1;99;0
WireConnection;135;1;158;0
WireConnection;137;1;158;0
WireConnection;83;0;84;0
WireConnection;83;1;98;0
WireConnection;140;1;158;0
WireConnection;174;0;135;4
WireConnection;174;1;149;4
WireConnection;138;1;158;0
WireConnection;192;0;83;0
WireConnection;192;1;193;0
WireConnection;163;0;137;0
WireConnection;163;1;142;0
WireConnection;178;0;174;0
WireConnection;178;1;192;0
WireConnection;152;0;151;0
WireConnection;152;1;138;4
WireConnection;154;0;140;0
WireConnection;154;1;153;0
WireConnection;162;0;135;0
WireConnection;162;1;149;0
WireConnection;148;1;163;0
WireConnection;146;0;147;0
WireConnection;146;1;138;0
WireConnection;0;0;162;0
WireConnection;0;2;148;0
WireConnection;0;3;146;0
WireConnection;0;4;152;0
WireConnection;0;5;154;0
WireConnection;0;9;178;0
ASEEND*/
//CHKSM=D08187F6BA66215A8E6AAD8AFF57850624B36B53